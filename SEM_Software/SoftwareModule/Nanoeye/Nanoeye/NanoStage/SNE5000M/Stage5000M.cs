using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SECtype = SEC.GenericSupport.DataType;
using SEC.Nanoeye.NanoStage.MMCValues;

namespace SEC.Nanoeye.NanoStage.SNE5000M
{
	class Stage5000M : MMCValues.MMCStage, IStage5000M
	{
		#region Property & Vairables
		System.Threading.Timer collisionTimer;

		long[] posMax = new long[] { 40000000, 40000000, 180000, 180000, 50000 };
		long[] posMin = new long[] { -40000000, -40000000, -180000, -180000, -50000 };

		protected enum AxisEnum : int
		{
			AxX = 0,
			AxY = 1,
			AxR = 2,
			AxT = 3,
			AxZ = 4
		}

		string[] axNameTable = new string[] { "AxX", "AxY", "AxR", "AxT", "AxZ" };

		public IAxis AxisX
		{
			get { return controls[axNameTable[(int)AxisEnum.AxX]] as IAxis; }
		}

		public IAxis AxisY
		{
			get { return controls[axNameTable[(int)AxisEnum.AxY]] as IAxis; }
		}

		public IAxis AxisR
		{
			get { return controls[axNameTable[(int)AxisEnum.AxR]] as IAxis; }
		}

		public IAxis AxisT
		{
			get { return controls[axNameTable[(int)AxisEnum.AxT]] as IAxis; }
		}

		public IAxis AxisZ
		{
			get { return controls[axNameTable[(int)AxisEnum.AxZ]] as IAxis; }
		}

		public override bool Enable
		{
			get { return base.Enable; }
			set
			{
				for (int i = 0; i < 5; i++)
				{
					controls[axNameTable[i]].Enable = value;
				}

				MMCValues.MMCAPICollection.IOPort.set_io((short)0, (uint)(value ? ~1 : 0xfffff));

				base.Enable = value;
			}
		}

		private CollisionAreaType _CollisionData = new CollisionAreaType(2000000, 500000, 29500000, 20000000);
		public CollisionAreaType CollisionData
		{
			get { return _CollisionData; }
			set { _CollisionData = value; }
		}

		private bool _UseCollisionPrevention = false;
		public bool UseCollisionPrevention
		{
			get { return _UseCollisionPrevention; }
			set
			{
				if (_UseCollisionPrevention != value)
				{
					_UseCollisionPrevention = value;
					if (_UseCollisionPrevention)
					{
						for (int i = 0; i < 5; i++)
						{
							posMax[i] = (controls[axNameTable[i]] as IAxis).Position.Maximum;
							posMin[i] = (controls[axNameTable[i]] as IAxis).Position.Minimum;
						}

						collisionTimer.Change(50, 50);
					}
					else
					{
						collisionTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
						for (int i = 0; i < 5; i++)
						{
							(controls[axNameTable[i]] as IAxis).Position.Maximum = posMax[i];
							(controls[axNameTable[i]] as IAxis).Position.Minimum = posMin[i];
						}
					}
					OnUseCollisionPrevention_ValueChanged();

					System.Diagnostics.Debug.WriteLine("Collision Prevention Changed - " + _UseCollisionPrevention.ToString(), _Name);
				}
			}
		}

		public override bool[] LimitSensor
		{
			get { throw new NotSupportedException(); }
		}
		#endregion

		#region 생성자와 초기화
		public Stage5000M()
		{
			collisionTimer = new System.Threading.Timer(new System.Threading.TimerCallback(CollisionChecker_Tick));
			collisionTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
		}

		public override void Initialize()
		{
			if (_Device == null) { throw new InvalidOperationException("사용할 Board가 정의되지 않았음."); }

			syncTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

			int addr = int.Parse(_Device);
			MMCValues.MMCAPICollection.ErrorControl.Assert(MMCValues.MMCAPICollection.Initialize.mmc_initx(1, ref addr));

			// 4축 보드로 Debuging 시 4로 설정 한다.
			int axCount = 5;

			if (MMCValues.MMCAPICollection.MMCConfig.mmc_all_axes() < axCount)
			{
				throw new InvalidOperationException("There are few axes.");
			}

			// 축 생성시 0번축의 데이터가 변경 되므로
			// 여기서 미리 생성함.
			MMCAxis ax;
			for (short i = 0; i < axCount; i++)
			{
				ax = new MMCAxis();
				ax.BeginInit();
				ax.Name = axNameTable[i];
				ax.Owner = this;
				ax.BoardNumber = 0;
				ax.AxisNumber = i;
				controls.Add(ax.Name, ax);
			}

			System.Diagnostics.Trace.WriteLine("Begin Initialize Stage5000M", "Info");

			MMCPosition axPos;
			SECtype.ControlLong cl;
			SECtype.ControlInt conInt;

			long[] posMaxTable = { 40000000L, 40000000L, 360000, 90000, 0 };
			long[] posMinTable = { -40000000L, -40000000L, -360000, -20000, -45000000 };
			long[] posOffsetTable = { 0, 0, 0, 0, 45000000 };
			long[] speedMaxTable = { 10000000, 10000000, 9000, 9000, 5000000 };
			long[] speedMinTable = { 100, 100, 100, 100, 250000 };
			int[] resolutionTable = { 400, 400, 400, 400, 800 };
			int[] stepTable = { 753600, 500000, 3666, 3600, 1000000 };
			long[] homeposTable = { 40000000, 40000000, 0, 0, 0 };

			for (short i = 0; i < axCount; i++)
			{
				System.Diagnostics.Debug.WriteLine("Begin Initialize Stage5000M - " + axNameTable[i], "Info");

				ax = controls[axNameTable[i]] as MMCAxis;
				ax.BeginInit();

				ax.HomePosition = homeposTable[i];

				ax.Resolution = resolutionTable[i];
				ax.StepDistance = stepTable[i];

				axPos = ax.Position as MMCPosition;
				axPos.DefaultMax = posMaxTable[i];
				axPos.DefaultMin = posMinTable[i];
				axPos.Maximum = posMaxTable[i];
				axPos.Minimum = posMinTable[i];
				axPos.Offset = posOffsetTable[i];
				//axPos.Value = (posMaxTable[i] + posMinTable[i]) / 2;	 MMC에서 읽어 옮.

				cl = ax.Speed as SECtype.ControlLong;
				cl.DefaultMax = speedMaxTable[i];
				cl.DefaultMin = speedMinTable[i];
				cl.Maximum = speedMaxTable[i];
				cl.Minimum = speedMinTable[i];
				cl.Value = speedMinTable[i];
				cl.Offset = 0;

				conInt = ax.AccelTime as SECtype.ControlInt;
				conInt.DefaultMax = 100000;
				conInt.DefaultMin = 100;
				conInt.Maximum = 100000;
				conInt.Minimum = 100;
				conInt.Offset = 0;
				conInt.Value = 500;

				conInt = ax.StopTime as SECtype.ControlInt;
				conInt.DefaultMax = 100000;
				conInt.DefaultMin = 100;
				conInt.Maximum = 100000;
				conInt.Minimum = 100;
				conInt.Offset = 0;
				conInt.Value = 500;

				conInt = ax.EmergencyStopTime as SECtype.ControlInt;
				conInt.DefaultMax = 100000;
				conInt.DefaultMin = 100;
				conInt.Maximum = 100000;
				conInt.Minimum = 100;
				conInt.Offset = 0;
				conInt.Value = 500;

				ax.EndInit();
			}

			syncTimer.Change(10, 10);

			_Initialized = true;

			System.Diagnostics.Trace.WriteLine("End Initialize", "Stage5000M");
		}

		public override void Dispose()
		{
			collisionTimer.Dispose();
			base.Dispose();
		}
		#endregion

		#region Event
		public event EventHandler UseCollisionPrevention_ValueChanged;
		protected virtual void OnUseCollisionPrevention_ValueChanged()
		{
			if (UseCollisionPrevention_ValueChanged != null)
			{
				UseCollisionPrevention_ValueChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		#region HomeSearch
		SEC.Nanoeye.NanoStage.HomeSearch hsX;
		SEC.Nanoeye.NanoStage.HomeSearch hsY;
		SEC.Nanoeye.NanoStage.HomeSearch hsR;
		SEC.Nanoeye.NanoStage.HomeSearch hsT;
		SEC.Nanoeye.NanoStage.HomeSearch hsZ;

		private bool preCollision = false;
		private bool cancelRequeset = false;

		public override void HomeSearch(bool value)
		{
			if (!value)
			{
				if (!_IsHomeSearching) { return; }

				cancelRequeset = true;

				if (hsX != null) { hsX.Stop(); hsX = null; }
				if (hsY != null) { hsY.Stop(); hsY = null; }
				if (hsR != null) { hsR.Stop(); hsR = null; }
				if (hsT != null) { hsT.Stop(); hsT = null; }
				if (hsZ != null) { hsZ.Stop(); hsZ = null; }


				WaitForAllAxesDone();
				_IsHomeSearching = false;
				IsHomeSearched = false;
				OnHomeSearchStateChanged();
				UseCollisionPrevention = preCollision;
			}
			else
			{
				preCollision = UseCollisionPrevention;

				UseCollisionPrevention = false;

				cancelRequeset = false;

				if (hsX != null) { throw new InvalidOperationException("HomeSearch Working!!!"); }
				if (hsY != null) { throw new InvalidOperationException("HomeSearch Working!!!"); }
				if (hsR != null) { throw new InvalidOperationException("HomeSearch Working!!!"); }
				if (hsT != null) { throw new InvalidOperationException("HomeSearch Working!!!"); }
				if (hsZ != null) { throw new InvalidOperationException("HomeSearch Working!!!"); }

				_IsHomeSearching = true;
				IsHomeSearched = false;
				OnHomeSearchStateChanged();

				hsX = new HomeSearch(AxisX as MMCAxis);
				hsX.MinimumSpeedModifier = _HomeSearchMinimumSpeedModifier;
				hsY = new HomeSearch(AxisY as MMCAxis);
				hsY.MinimumSpeedModifier = _HomeSearchMinimumSpeedModifier;
				hsR = new HomeSearch(AxisR as MMCAxis);
				hsR.MinimumSpeedModifier = 1;
				hsT = new HomeSearch(AxisT as MMCAxis);
				hsT.MinimumSpeedModifier = 1;
				hsZ = new HomeSearch(AxisZ as MMCAxis);
				hsZ.MinimumSpeedModifier = 1;

				hsX.HomeSearchDone += new EventHandler(hsX_HomeSearchDone);
				hsY.HomeSearchDone += new EventHandler(hsY_HomeSearchDone);
				hsR.HomeSearchDone += new EventHandler(hsR_HomeSearchDone);
				hsT.HomeSearchDone += new EventHandler(hsT_HomeSearchDone);
				hsZ.HomeSearchDone += new EventHandler(hsZ_HomeSearchDone);

				hsR.Start(SEC.Nanoeye.NanoStage.HomeSearch.HomeSearchType.NoSensor);
				hsZ.Start(SEC.Nanoeye.NanoStage.HomeSearch.HomeSearchType.PosSensor);
			}

			

		}

		void hsZ_HomeSearchDone(object sender, EventArgs e)
		{
			if (cancelRequeset) { System.Diagnostics.Debug.WriteLine("Cancel Requested. hsZ", _Name); return; }

			hsX.Start(SEC.Nanoeye.NanoStage.HomeSearch.HomeSearchType.NegSensor);
			hsY.Start(SEC.Nanoeye.NanoStage.HomeSearch.HomeSearchType.NegSensor);
			hsT.Start(SEC.Nanoeye.NanoStage.HomeSearch.HomeSearchType.HomeSensor);

			CheckHomeSearchDone();
		}

		void hsR_HomeSearchDone(object sender, EventArgs e)
		{
			if (cancelRequeset) { System.Diagnostics.Debug.WriteLine("Cancel Requested. hsR", _Name); return; }

			CheckHomeSearchDone();
		}

		void hsY_HomeSearchDone(object sender, EventArgs e)
		{
			if (cancelRequeset) { System.Diagnostics.Debug.WriteLine("Cancel Requested. hsY", _Name); return; }

			CheckHomeSearchDone();
		}

		void hsX_HomeSearchDone(object sender, EventArgs e)
		{
			if (cancelRequeset) { System.Diagnostics.Debug.WriteLine("Cancel Requested. hsX", _Name); return; }

			CheckHomeSearchDone();
		}

		void hsT_HomeSearchDone(object sender, EventArgs e)
		{
			if (cancelRequeset) { System.Diagnostics.Debug.WriteLine("Cancel Requested. hsT", _Name); return; }

			CheckHomeSearchDone();
		}

		private void CheckHomeSearchDone()
		{
			if ((hsX == null) || (hsY == null) || (hsR == null) || (hsT == null) || (hsZ == null)) { return; }

			if (!hsX.IsSearched) { System.Diagnostics.Debug.WriteLine("HomeSearch Not Complete by X.", _Name); return; }
			if (!hsY.IsSearched) { System.Diagnostics.Debug.WriteLine("HomeSearch Not Complete by Y.", _Name); return; }
			if (!hsR.IsSearched) { System.Diagnostics.Debug.WriteLine("HomeSearch Not Complete by R.", _Name); return; }
			if (!hsT.IsSearched) { System.Diagnostics.Debug.WriteLine("HomeSearch Not Complete by T.", _Name); return; }
			if (!hsZ.IsSearched) { System.Diagnostics.Debug.WriteLine("HomeSearch Not Complete by Z.", _Name); return; }

			hsX = null;
			hsY = null;
			hsR = null;
			hsT = null;
			hsZ = null;

			_IsHomeSearching = false;
			IsHomeSearched = true;

			System.Diagnostics.Debug.WriteLine("HomeSearch Complete.", _Name);

			UseCollisionPrevention = preCollision;

			AxisX.Enable = true;
			AxisY.Enable = true;
			AxisR.Enable = true;
			AxisT.Enable = true;
			AxisZ.Enable = true;

			AxisX.Enable = !AxisX.AutoOff;
			AxisY.Enable = !AxisY.AutoOff;
			AxisR.Enable = !AxisR.AutoOff;
			AxisT.Enable = !AxisT.AutoOff;
			AxisZ.Enable = !AxisZ.AutoOff;
		}
		#endregion

		#region Collision Check

		void CollisionChecker_Tick(object state)
		{
			Check_Door();
			Check_Tilt();
		}

		private void Check_Tilt()
		{
			//if(AxisT.IsMotion)
			//{
			// Z축 제한.
			// 현재 Tilt 각도에서 Z축을 제한 해야 함.

			double angelBase = Math.Atan2(_CollisionData.TiltRadius, _CollisionData.TiltHeight);
			double virtualRadius = Math.Sqrt(_CollisionData.TiltRadius * _CollisionData.TiltRadius + _CollisionData.TiltHeight * _CollisionData.TiltHeight);


			double angelNew = Math.PI / 2 - angelBase + Math.Abs(AxisT.Position.Value) * Math.PI / 1000 / 180;

			double t1 = Math.Sin(angelNew);
			double t2 = t1 * virtualRadius;
			double t3 = Math.Abs(t2);
			double t4 = t3 - _CollisionData.TiltHeight;

			double limitH = t4;

			//double limitH = Math.Abs(Math.Sin(angelNew) * virtualRadius) - _CollisionData.TiltHeight;

			if (limitH < 0) { limitH = 0; }

			if (AxisZ.Position.Minimum != (long)limitH) { AxisZ.Position.Minimum = (long)limitH; }

			// T축 제한.
			// 현재 Z축 높이에서 최대 Tilt 각도를 제한 해야 함.

			angelBase = Math.Atan2(_CollisionData.TiltHeight, _CollisionData.TiltRadius); // radius
			virtualRadius = Math.Sqrt(_CollisionData.TiltRadius * _CollisionData.TiltRadius + _CollisionData.TiltHeight * _CollisionData.TiltHeight);	// nm

			long tMax, tMin;

			// 어떠한 각도에서도 걸리지 않음.
			if ((virtualRadius - _CollisionData.TiltHeight) < AxisZ.Position.Value)
			{
				tMax = posMax[(int)AxisEnum.AxT];
				tMin = posMin[(int)AxisEnum.AxT];
			}
			else
			{
				double limitAngle = Math.Asin((AxisZ.Position.Value + _CollisionData.TiltHeight) / virtualRadius) - angelBase;	// radius

				double limitConveted = Math.Abs(limitAngle * 180 / Math.PI);


				if (double.IsNaN(limitConveted))
				{
					tMax = posMax[(int)AxisEnum.AxT];
					tMin = posMin[(int)AxisEnum.AxT];
				}
				else
				{
					tMax = (long)(limitConveted * 1000);
					tMin = (long)(limitConveted * -1000);
				}
			}

			if (AxisT.Position.Maximum != tMax) { AxisT.Position.Maximum = tMax; }
			if (AxisT.Position.Minimum != tMin) { AxisT.Position.Minimum = tMin; }
		}

		private void Check_Door()
		{
			long pos;

			if (AxisX.Position.Value > (posMax[(int)AxisEnum.AxX] - CollisionData.DoorXRight)) { pos = posMin[(int)AxisEnum.AxY] + CollisionData.DoorYBottom; }
			else { pos = posMin[(int)AxisEnum.AxY]; }
			if (AxisY.Position.Minimum != pos) { AxisY.Position.Minimum = pos; }

			if (AxisY.Position.Value < (posMin[(int)AxisEnum.AxY] + CollisionData.DoorYBottom)) { pos = posMax[(int)AxisEnum.AxX] - CollisionData.DoorXRight; }
			else { pos = posMax[(int)AxisEnum.AxX]; }
			if (AxisX.Position.Maximum != pos) { AxisX.Position.Maximum = pos; }
		}

		#endregion

		private void WaitForAllAxesDone()
		{
			while (AxisX.IsMotion)
			{
				System.Threading.Thread.Sleep(10);
				System.Diagnostics.Debug.WriteLine("Wait for AxX end.");
			}
			while (AxisY.IsMotion)
			{
				System.Threading.Thread.Sleep(10);
				System.Diagnostics.Debug.WriteLine("Wait for AxY end.");
			}

			while (AxisR.IsMotion)
			{
				System.Threading.Thread.Sleep(10);
				System.Diagnostics.Debug.WriteLine("Wait for AxR end.");
			}

			while (AxisT.IsMotion)
			{
				System.Threading.Thread.Sleep(10);
				System.Diagnostics.Debug.WriteLine("Wait for AxT end.");
			}

			while (AxisZ.IsMotion)
			{
				System.Threading.Thread.Sleep(10);
				System.Diagnostics.Debug.WriteLine("Wait for AxZ end.");
			}

		}
	}
}
