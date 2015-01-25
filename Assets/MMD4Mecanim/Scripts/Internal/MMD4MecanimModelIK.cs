using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MorphCategory		= MMD4MecanimData.MorphCategory;
using MorphType			= MMD4MecanimData.MorphType;
using MorphData			= MMD4MecanimData.MorphData;
using MorphMotionData	= MMD4MecanimData.MorphMotionData;
using BoneData			= MMD4MecanimData.BoneData;
using IKData			= MMD4MecanimData.IKData;
using IKLinkData		= MMD4MecanimData.IKLinkData;
using FileType			= MMD4MecanimData.FileType;

using Bone				= MMD4MecanimBone;

public partial class MMD4MecanimModel
{
	[System.NonSerialized]
	public MMD4MecanimIKTarget[] fullBodyIKTargetList;
	[System.NonSerialized]
	public Transform fullBodyIKTargets;
	[System.NonSerialized]
	public Transform[] fullBodyIKGroupList;

	public class IK
	{
		int					_ikID;
		IKData				_ikData;

		public int ikID { get { return _ikID; } }
		public IKData ikData { get { return _ikData; } }

		public class IKLink
		{
			public IKLinkData	ikLinkData;
			public Bone			bone;
		}

		Bone				_destBone;
		Bone				_targetBone;
		IKLink[]			_ikLinkList;

		public Bone destBone { get { return _destBone; } }
		public Bone targetBone { get { return _targetBone; } }
		public IKLink[] ikLinkList { get { return _ikLinkList; } }

		public bool ikEnabled {
			get {
				if(_destBone != null ) {
					return _destBone.ikEnabled;
				}
				return false;
			}
			set {
				if( _destBone != null ) {
					_destBone.ikEnabled = value;
				}
			}
		}

		public float ikWeight {
			get {
				if(_destBone != null ) {
					return _destBone.ikWeight;
				}
				return 0.0f;
			}
			set {
				if( _destBone != null ) {
					_destBone.ikWeight = value;
				}
			}
		}

		public IK( MMD4MecanimModel model, int ikID )
		{
			if( model == null || model.modelData == null || model.modelData.ikDataList == null ||
			    ikID >= model.modelData.ikDataList.Length ) {
				Debug.LogError("");
				return;
			}
			
			_ikID	= ikID;
			_ikData	= model.modelData.ikDataList[ikID];

			if( _ikData != null ) {
				_destBone = model.GetBone( _ikData.destBoneID );
				_targetBone = model.GetBone( _ikData.targetBoneID );
				if( _ikData.ikLinkDataList != null ) {
					_ikLinkList = new IKLink[_ikData.ikLinkDataList.Length];
					for( int i = 0; i < _ikData.ikLinkDataList.Length; ++i ) {
						_ikLinkList[i] = new IKLink();
						_ikLinkList[i].ikLinkData = _ikData.ikLinkDataList[i];
					}
				}
			}
		}

		public void Destroy()
		{
			_ikData = null;
			_destBone = null;
			_targetBone = null;
			_ikLinkList = null;
		}
	}

	void _InitializeIK()
	{
		_CheckTransformIK();
	}

	void _CheckTransformIK()
	{
		if( !Application.isPlaying ) {
			return;
		}

		if( !this.fullBodyIKEnabled ) {
			return;
		}

		if( this.fullBodyIKTargetList != null ) {
			if( this.fullBodyIKTargetList.Length != MMD4MecanimData.FullBodyIKTargetMax ) {
				this.fullBodyIKTargetList = null;
			} else {
				foreach( var target in this.fullBodyIKTargetList ) {
					if( target == null ) {
						this.fullBodyIKTargetList = null;
						break;
					}
				}
			}
		}

		if( this.fullBodyIKTargetList == null ) {
			if( this._bulletPhysicsMMDModel == null ) {
				return;
			}

			int[] fullBodyIKParentBoneIDList	= this._bulletPhysicsMMDModel.fullBodyIKParentBoneIDList;
			Vector3[] fullBodyIKPositionList	= this._bulletPhysicsMMDModel.fullBodyIKPositionList;
			Quaternion[] fullBodyIKRotationList	= this._bulletPhysicsMMDModel.fullBodyIKRotationList;

			if( fullBodyIKParentBoneIDList == null ||
				fullBodyIKPositionList == null ||
				fullBodyIKRotationList == null ) {
				return;
			}

			if( fullBodyIKParentBoneIDList.Length != MMD4MecanimData.FullBodyIKTargetMax ||
				fullBodyIKPositionList.Length != MMD4MecanimData.FullBodyIKTargetMax ||
				fullBodyIKRotationList.Length != MMD4MecanimData.FullBodyIKTargetMax ) {
				return;
			}

			if( this.fullBodyIKTargets == null ) {
				GameObject go = new GameObject("FullBodyIKTargets");
				this.fullBodyIKTargets = go.transform;
				this.fullBodyIKTargets.parent = this.transform;
				this.fullBodyIKTargets.localPosition = Vector3.zero;
				this.fullBodyIKTargets.localRotation = Quaternion.identity;
				this.fullBodyIKTargets.localScale = Vector3.one;
			}

			// Pending: Exclude fingers.
			if( this.fullBodyIKGroupList == null || this.fullBodyIKGroupList.Length != (int)MMD4MecanimData.FullBodyIKGroup.Max ) {
				this.fullBodyIKGroupList = new Transform[(int)MMD4MecanimData.FullBodyIKGroup.Max];
				for( int i = 0; i != (int)MMD4MecanimData.FullBodyIKGroup.Max; ++i ) {
					if( i != (int)MMD4MecanimData.FullBodyIKGroup.LeftFinger &&
						i != (int)MMD4MecanimData.FullBodyIKGroup.RightFinger ) {
						GameObject go = new GameObject( ((MMD4MecanimData.FullBodyIKGroup)i).ToString() );
						this.fullBodyIKGroupList[i] = go.transform;
						this.fullBodyIKGroupList[i].parent = this.fullBodyIKTargets;
						this.fullBodyIKGroupList[i].localPosition = Vector3.zero;
						this.fullBodyIKGroupList[i].localRotation = Quaternion.identity;
						this.fullBodyIKGroupList[i].localScale = Vector3.one;
					}
				}
			}

			MMD4MecanimBone rootBone = GetRootBone();
			Transform rootBoneTransform = null;
			if( rootBone != null ) {
				rootBoneTransform = rootBone.transform;
			}

			this.fullBodyIKTargetList = new MMD4MecanimIKTarget[MMD4MecanimData.FullBodyIKTargetMax];
			for( int i = 0; i != MMD4MecanimData.FullBodyIKTargetMax; ++i ) {
				MMD4MecanimData.FullBodyIKGroup fullBodyIKGroup;
				string fullBodyIKTargetName = MMD4MecanimData.GetFullBodyIKTargetName( out fullBodyIKGroup, i );
				if( fullBodyIKGroup != MMD4MecanimData.FullBodyIKGroup.Unknown ) {
					GameObject go = new GameObject( fullBodyIKTargetName );
					Transform t = go.transform;

					Vector3 position = fullBodyIKPositionList[i];
					Quaternion rotation = fullBodyIKRotationList[i];
					if( rootBoneTransform != null ) {
						position = rootBoneTransform.TransformPoint( position );
						rotation = rootBoneTransform.rotation * rotation;
					}

					MMD4MecanimBone parentBone = GetBone( fullBodyIKParentBoneIDList[i] );
					if( parentBone != null ) {
						t.parent = parentBone.transform;
						go.name = "FBIK." + fullBodyIKGroup.ToString() + "." + go.name;
					} else {
						t.parent = this.fullBodyIKGroupList[(int)fullBodyIKGroup];
					}

					t.position = position;
					t.rotation = rotation;
					t.localScale = Vector3.one;
					this.fullBodyIKTargetList[i] = go.AddComponent< MMD4MecanimIKTarget >();
					this.fullBodyIKTargetList[i].model = this;
				}
			}
		}
	}

	void _UpdateIK()
	{
		_CheckTransformIK();
	}

	void _LateUpdateIK()
	{
		if( !Application.isPlaying ) {
			return;
		}

		if( this._bulletPhysicsMMDModel == null ) {
			return;
		}

		int[]		updateFullBodyIKFlagsList		= this._bulletPhysicsMMDModel.updateFullBodyIKFlagsList;
		Matrix4x4[]	updateFullBodyIKTransformList	= this._bulletPhysicsMMDModel.updateFullBodyIKTransformList;
		float[]		updateFullBodyIKWeightList		= this._bulletPhysicsMMDModel.updateFullBodyIKWeightList;

		if( updateFullBodyIKFlagsList == null ||
			updateFullBodyIKTransformList == null ||
			updateFullBodyIKWeightList == null ) {
			return;
		}

		if( updateFullBodyIKFlagsList.Length != MMD4MecanimData.FullBodyIKTargetMax ||
			updateFullBodyIKTransformList.Length != MMD4MecanimData.FullBodyIKTargetMax ||
			updateFullBodyIKWeightList.Length != MMD4MecanimData.FullBodyIKTargetMax ) {
			return;
		}

		if( this.fullBodyIKTargetList != null && this.fullBodyIKTargetList.Length == MMD4MecanimData.FullBodyIKTargetMax ) {
			for( int i = 0; i != MMD4MecanimData.FullBodyIKTargetMax; ++i ) {
				MMD4MecanimIKTarget ikTarget = this.fullBodyIKTargetList[i];
				if( ikTarget != null ) {
					// updateFullBodyIKFlagsList / isDisabled
					updateFullBodyIKTransformList[i] = ikTarget.localToWorldMatrix;
					updateFullBodyIKWeightList[i] = ikTarget.ikWeight;
				}
			}
		}
	}
}
