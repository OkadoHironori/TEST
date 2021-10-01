using System;

namespace SensorTechnology
{
	/// <summary>
	/// CStCamera
	/// </summary>
	public class CStCamera : IDisposable
	{
		private IntPtr m_hCamera = IntPtr.Zero;

		private IntPtr m_hPreviewParentWnd = IntPtr.Zero;

		//Camera ID
		private uint m_nCameraID = 0;
		private string m_strCameraName = "";

		//Original Imaga Info
		private ushort m_wScanMode = StCam.STCAM_SCAN_MODE_NORMAL;
		private uint m_dwOrgImageWidth = 0;
		private uint m_dwOrgImageHeight = 0;

		//Display Mode
		private byte m_byteDisplayMode = StCam.STCAM_DISPLAY_MODE_GDI;

		//Aspect Mode
		private byte m_byteAspectMode = StCam.STCAM_ASPECT_MODE_FIXED;

		//Preview Window
		private int m_lPreviewWindowOffsetX = 0;
		private int m_lPreviewWindowOffsetY = 0;
		private uint m_dwPreviewWindowWidth = 0;
		private uint m_dwPreviewWindowHeight = 0;

		//Preview Mask
		private uint m_dwPreviewMaskOffsetX = 0;
		private uint m_dwPreviewMaskOffsetY = 0;
		private uint m_dwPreviewMaskWidth = 0;
		private uint m_dwPreviewMaskHeight = 0;

		//Preview Dest
		private uint m_dwPreviewDestOffsetX = 0;
		private uint m_dwPreviewDestOffsetY = 0;
		private uint m_dwPreviewDestWidth = 0;
		private uint m_dwPreviewDestHeight = 0;

		//---------------------------
		//Functions
		//---------------------------

		public CStCamera()
		{
			// 
			// 
			//
		}
		//---------------------------
		#region IDisposable
		public void Dispose()
		{
			if(m_hCamera != IntPtr.Zero)
			{
				StCam.Close(m_hCamera);
				m_hCamera = IntPtr.Zero;
			}
		}
		#endregion
		
		//---------------------------
		public override string ToString()
		{
			return(m_strCameraName);
		}

		//---------------------------
		#region Open/Close
		public bool Open()
		{
			bool result = true;
			Close();
			m_hCamera = StCam.Open(0);
			if(m_hCamera != IntPtr.Zero)
			{
				result = mGetSettings();
				if(!result)
				{
					Close();
				}
			}
			else
			{
				result = false;
			}
			return(result);
		}
		public void Close()
		{
			Dispose();
		}
		#endregion Open/Close


		//---------------------------
		private bool mGetSettings()
		{
			bool result = true;

			do
			{
				result = mGetCameraName();
				if(!result) break;

				result = mGetImageSize();
				if(!result) break;

				result = mGetDisplayMode();
				if(!result) break;

				result = mGetAspectMode();
				if(!result) break;

				result = mGetPreviewMaskSize();
				if(!result) break;

				result = mGetPreviewDestSize();
				if(!result) break;

			}while(false);
			return(result);
		}

		//---------------------------
		public bool CreateWindow(IntPtr hWnd)
        {
			bool result = true;
			UInt32 dwStyle = StCam.WS_VISIBLE | StCam.WS_HSCROLL | StCam.WS_VSCROLL;

			if(m_hCamera == IntPtr.Zero)
			{
				result = false;
			}
			else
			{
				if(hWnd == IntPtr.Zero)
				{
					//Overlapped Window
					dwStyle |= StCam.WS_OVERLAPPEDWINDOW;
					m_hPreviewParentWnd = IntPtr.Zero;
				}
				else
				{
					//Child Window
					dwStyle |= StCam.WS_CHILDWINDOW;
					m_hPreviewParentWnd = hWnd;
				}

				result =StCam.CreatePreviewWindow(m_hCamera, "Preview", dwStyle,
					m_lPreviewWindowOffsetX, m_lPreviewWindowOffsetY, m_dwPreviewWindowWidth, m_dwPreviewWindowHeight,
					hWnd, IntPtr.Zero, false);
				if(result)
				{
					result = mGetPreviewWindowSize();
				}
			}		
											 
			return(result);
		}

		//---------------------------
		#region Start/StopTransfer
		public bool StartTransfer()
		{
			bool result = true;

			if(m_hCamera == IntPtr.Zero)
			{
				result = false;
			}
			else
			{
				result = StCam.StartTransfer(m_hCamera);
			}
			return(result);
		}
		public bool StopTransfer()
		{
			bool result = true;

			if(m_hCamera == IntPtr.Zero)
			{
				result = false;
			}
			else
			{
				result = StCam.StopTransfer(m_hCamera);
			}
			return(result);
		}
		#endregion Start/StopTransfer

		//---------------------------
		#region CameraName
		private bool mGetCameraName()
		{
			bool result = true;
			uint dwCameraID = 0;
			System.Text.StringBuilder sb = new System.Text.StringBuilder(255);

			result = StCam.ReadCameraUserID(m_hCamera, out dwCameraID, sb, (uint)sb.Capacity);
			if(result)
			{
				m_nCameraID = dwCameraID;
				m_strCameraName = sb.ToString();
			}
			return(result);
		}
		public string CameraName
		{
			get{return(m_strCameraName);}
		}
		#endregion CameraName
		//---------------------------
		#region ImageSize
		private bool mGetImageSize()
		{
			bool result = true;

            uint dwSize = 0;
            uint dwLinePitch = 0;
            result = StCam.GetPreviewDataSize(m_hCamera, out dwSize, out m_dwOrgImageWidth, out m_dwOrgImageHeight, out dwLinePitch);
			return(result);
		}
		public uint OrgImageWidth
		{
			get{return(m_dwOrgImageWidth);}
		}
		public uint OrgImageHeight
		{
			get{return(m_dwOrgImageHeight);}
		}
		#endregion ImageSize
		//---------------------------
		#region DisplayMode
		private bool mGetDisplayMode()
		{
			bool result = true;
			result = StCam.GetDisplayMode(m_hCamera, out m_byteDisplayMode);
			return(result);
		}
		private bool mSetDisplayMode(byte byteDisplayMode)
		{
			bool result = true;
			do
			{
				result = StCam.DestroyPreviewWindow(m_hCamera);
				if(!result) break;

				result = StCam.SetDisplayMode(m_hCamera, byteDisplayMode);
				if(!result) break;

				m_byteDisplayMode = byteDisplayMode;

				result = CreateWindow(m_hPreviewParentWnd);
				if(!result) break;
			}while(false);
			return(result);
		}
		public byte DisplayMode
		{
			get{return(m_byteDisplayMode);}
			set{mSetDisplayMode(value);}
		}	
		#endregion DisplayMode

		//---------------------------
		#region AspectMode
		private bool mGetAspectMode()
		{
			bool result = true;
			result = StCam.GetAspectMode(m_hCamera, out m_byteAspectMode);
			return(result);
		}
		private bool mSetAspectMode(byte byteAspectMode)
		{
			bool result = true;
			result = StCam.SetAspectMode(m_hCamera, byteAspectMode);
			if(result)
			{
				m_byteAspectMode = byteAspectMode;

				result = mGetSettings();
			}
			return(result);
		}
		public byte AspectMode
		{
			get{return(m_byteAspectMode);}
			set{mSetAspectMode(value);}
		}
		#endregion AspectMode

		//---------------------------
		#region PreviewWindowSize
		private bool mGetPreviewWindowSize()
		{
			bool result = true;
			result = StCam.GetPreviewWindowSize(m_hCamera, out m_lPreviewWindowOffsetX, out m_lPreviewWindowOffsetY, out  m_dwPreviewWindowWidth, out m_dwPreviewWindowHeight);
			return(result);
		}
		private bool mSetPreviewWindowSize(int nOffsetX, int nOffsetY, UInt32 dwWidth, UInt32 dwHeight)
		{
			bool result = true;
			result = StCam.SetPreviewWindowSize(m_hCamera, nOffsetX, nOffsetY, dwWidth, dwHeight);
			if(result)
			{
				m_lPreviewWindowOffsetX = nOffsetX;
				m_lPreviewWindowOffsetY = nOffsetY;
				m_dwPreviewWindowWidth = dwWidth;
				m_dwPreviewWindowHeight = dwHeight;
			}
			return(result);
		}
		public int PreviewWindowOffsetX
		{
			get{return(m_lPreviewWindowOffsetX);}
			set{mSetPreviewWindowSize(value, m_lPreviewWindowOffsetY, m_dwPreviewWindowWidth, m_dwPreviewWindowHeight);}
		}
		public int PreviewWindowOffsetY
		{
			get{return(m_lPreviewWindowOffsetY);}
			set{mSetPreviewWindowSize(m_lPreviewWindowOffsetX, value, m_dwPreviewWindowWidth, m_dwPreviewWindowHeight);}
		}
		public uint PreviewWindowWidth
		{
			get{return(m_dwPreviewWindowWidth);}
			set
			{
				mSetPreviewWindowSize(m_lPreviewWindowOffsetX, m_lPreviewWindowOffsetY, value, m_dwPreviewWindowHeight);
			}
		}
		public uint PreviewWindowHeight
		{
			get{return(m_dwPreviewWindowHeight);}
			set
			{
				mSetPreviewWindowSize(m_lPreviewWindowOffsetX, m_lPreviewWindowOffsetY, m_dwPreviewWindowWidth, value);
			}
		}
		#endregion PreviewWindowSize

		//---------------------------
		#region PreviewMaskSize
		private bool mGetPreviewMaskSize()
		{
			bool result = true;
			result = StCam.GetPreviewMaskSize(m_hCamera, out m_dwPreviewMaskOffsetX, out m_dwPreviewMaskOffsetY, out m_dwPreviewMaskWidth, out m_dwPreviewMaskHeight);
			return(result);
		}
		private bool mSetPreviewMaskSize(UInt32 nOffsetX, UInt32 nOffsetY, UInt32 dwWidth, UInt32 dwHeight)
		{
			bool result = true;
			result = StCam.SetPreviewMaskSize(m_hCamera, nOffsetX, nOffsetY, dwWidth, dwHeight);
			if(result)
			{
				m_dwPreviewMaskOffsetX = nOffsetX;
				m_dwPreviewMaskOffsetY = nOffsetY;
				m_dwPreviewMaskWidth = dwWidth;
				m_dwPreviewMaskHeight = dwHeight;
			}
			return(result);
		}
		public UInt32 PreviewMaskOffsetX
		{
			get{return(m_dwPreviewMaskOffsetX);}
			set{mSetPreviewMaskSize(value, m_dwPreviewMaskOffsetY, m_dwPreviewMaskWidth, m_dwPreviewMaskHeight);}
		}
		public UInt32 PreviewMaskOffsetY
		{
			get{return(m_dwPreviewMaskOffsetY);}
			set{mSetPreviewMaskSize(m_dwPreviewMaskOffsetX, value, m_dwPreviewMaskWidth, m_dwPreviewMaskHeight);}
		}
		public uint PreviewMaskWidth
		{
			get{return(m_dwPreviewMaskWidth);}
			set{mSetPreviewMaskSize(m_dwPreviewMaskOffsetX, m_dwPreviewMaskOffsetY, value, m_dwPreviewMaskHeight);}
		}
		public uint PreviewMaskHeight
		{
			get{return(m_dwPreviewMaskHeight);}
			set{mSetPreviewMaskSize(m_dwPreviewMaskOffsetX, m_dwPreviewMaskOffsetY, m_dwPreviewMaskWidth, value);}
		}
		#endregion PreviewMaskSize

		//---------------------------
		#region PreviewDestSize
		private bool mGetPreviewDestSize()
		{
			bool result = true;
			result = StCam.GetPreviewDestSize(m_hCamera,out  m_dwPreviewDestOffsetX, out m_dwPreviewDestOffsetY, out m_dwPreviewDestWidth, out m_dwPreviewDestHeight);
			return(result);
		}
		private bool mSetPreviewDestSize(UInt32 nOffsetX, UInt32 nOffsetY, UInt32 dwWidth, UInt32 dwHeight)
		{
			bool result = true;
			result = StCam.SetPreviewDestSize(m_hCamera, nOffsetX, nOffsetY, dwWidth, dwHeight);
			if(result)
			{
				m_dwPreviewDestOffsetX = nOffsetX;
				m_dwPreviewDestOffsetY = nOffsetY;
				m_dwPreviewDestWidth = dwWidth;
				m_dwPreviewDestHeight = dwHeight;
			}
			return(result);
		}
		public UInt32 PreviewDestOffsetX
		{
			get{return(m_dwPreviewDestOffsetX);}
			set{mSetPreviewDestSize(value, m_dwPreviewDestOffsetY, m_dwPreviewDestWidth, m_dwPreviewDestHeight);}
		}
		public UInt32 PreviewDestOffsetY
		{
			get{return(m_dwPreviewDestOffsetY);}
			set{mSetPreviewDestSize(m_dwPreviewDestOffsetX, value, m_dwPreviewDestWidth, m_dwPreviewDestHeight);}
		}
		public uint PreviewDestWidth
		{
			get{return(m_dwPreviewDestWidth);}
			set{mSetPreviewDestSize(m_dwPreviewDestOffsetX, m_dwPreviewDestOffsetY, value, m_dwPreviewDestHeight);}
		}
		public uint PreviewDestHeight
		{
			get{return(m_dwPreviewDestHeight);}
			set{mSetPreviewDestSize(m_dwPreviewDestOffsetX, m_dwPreviewDestOffsetY, m_dwPreviewDestWidth, value);}
		}
		#endregion PreviewDestSize


        #region SaveCfgFile
        public uint mSaveCfgFile()
        {
            bool bret = false;
            uint iret = 0;

            bret = StCam.SaveSettingFile(m_hCamera, @"C:\CT\ScanCorrect\ExObsCam.cfg");

            if (bret == false)
            {
                iret = StCam.GetLastError(m_hCamera);
            }

            return (iret);
        }
        #endregion


        #region LoadCfgFile
        public uint mLoadCfgFile()
        {
            bool bret = false;
            uint iret = 0;

            bret = StCam.LoadSettingFile(m_hCamera, @"C:\CT\ScanCorrect\ExObsCam.cfg");

            if (bret == false)
            {
                iret = StCam.GetLastError(m_hCamera);
            }

            return (iret);
        }
        #endregion

        #region getCamHandle
        public IntPtr mGetCamHandle(CStCamera StCamera)
        {
            return (StCamera.m_hCamera);
        }
        #endregion 



    }
}
