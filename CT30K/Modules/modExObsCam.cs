using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Windows.Forms;
using SensorTechnology;
using System.Collections;

using CT30K.Properties;
using CT30K.Common;
using CTAPI;

namespace CT30K
{
    public class modExObsCam
    {
        private ArrayList m_arrayStCamera = new ArrayList(127);
        private int m_keepOpenIndex = -1;

        public bool mOpenAllCamera()
        {
            //bool result = true;
            //falseからstart
            bool result = false;

            for (int i = 0; i < m_arrayStCamera.Capacity; i++)
            {
                CStCamera stCamera = new CStCamera();
                if (stCamera.Open())
                {
                    m_arrayStCamera.Add(stCamera);
                }
                else
                {
                    break;
                }
            }

            if (m_arrayStCamera.Count == 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return (result);
        }

        public bool mCloseCamera()
        {
            bool result = true;
            //while ((m_keepOpenIndex < m_arrayStCamera.Count - 1) && (0 < m_arrayStCamera.Count))
            while (0 < m_arrayStCamera.Count)
            {
                CStCamera stCamera = m_arrayStCamera[m_arrayStCamera.Count - 1] as CStCamera;
                m_arrayStCamera.RemoveAt(m_arrayStCamera.Count - 1);
                stCamera.Dispose();
            }

            while (1 < m_arrayStCamera.Count)
            {
                CStCamera stCamera = m_arrayStCamera[0] as CStCamera;
                m_arrayStCamera.RemoveAt(0);
                stCamera.Dispose();
            }

            return (result);
        }

        public CStCamera OpendCamera
        {
            get
            {
                CStCamera stCamera = null;
                if (0 < m_arrayStCamera.Count)
                {
                    stCamera = m_arrayStCamera[0] as CStCamera;
                }

                return (stCamera);
            }
        }

        public uint LoadSettingFile(CStCamera StCamera)
        {
            uint ret = 0;
            ret = StCamera.mLoadCfgFile();

            return (ret);
        }

        public uint SaveSettingFile(CStCamera StCamera)
        {
            uint ret = 0;
            ret = StCamera.mSaveCfgFile();

            return (ret);
        }

        public IntPtr GetCamHandle(CStCamera StCamera)
        {
            return (StCamera.mGetCamHandle(StCamera));
        }


        public void ExObsCamProcessStart(bool stop = true,int DispFlg = 0)
        {
            frmExObsCam.Instance.DispFlg = DispFlg;

            //カメラハンドルがクローズしている場合は表示
            if (frmExObsCam.Instance.CamHandle == IntPtr.Zero) //カメラオープンしていない場合
            {
                bool ret = false;
                ret = frmExObsCam.Instance.OpenCamera();

                if (ret == false)
                {
                    MessageBox.Show(CTResources.LoadResString(24011));
                    return;
                }

                Application.DoEvents();

                if (DispFlg == 0)
                {
                    frmExObsCam.Instance.Show(frmCTMenu.Instance);
                    frmExObsCam.Instance.FrmLockFlg = 0;
                }
                else if (DispFlg == 1)
                {
                    frmCTMenu.Instance.ShowChild(frmExObsCam.Instance, frmScanControl.Instance.Right, 0);
                    frmExObsCam.Instance.FrmLockFlg = 1;
                }
                
                frmExObsCam.Instance.CaptureStart();

            }
            else if(stop == true && frmExObsCam.Instance.CamHandle != IntPtr.Zero) //カメラオープン時、位置指定モードでもない。
            {
                frmExObsCam.Instance.CaptureStop();

                Application.DoEvents();

                frmExObsCam.Instance.CloseCamera();

                Application.DoEvents();

                frmExObsCam.Instance.Close();
                frmExObsCam.Instance.Dispose();
            }
            else if (stop == false && frmExObsCam.Instance.CamHandle != IntPtr.Zero && DispFlg == 1) //カメラオープン時、位置指定モードに切り替えたい。
            {
                frmExObsCam.Instance.CaptureStop();
                frmExObsCam.Instance.CloseCamera();
                frmExObsCam.Instance.Close();
                frmExObsCam.Instance.Dispose();

                modCT30K.PauseForDoEvents(0.1f);

                frmExObsCam.Instance.DispFlg = DispFlg;

                bool ret = false;
                ret = frmExObsCam.Instance.OpenCamera();

                if (ret == false)
                {
                    MessageBox.Show(CTResources.LoadResString(24011));
                    return;
                }

                Application.DoEvents();

                frmCTMenu.Instance.ShowChild(frmExObsCam.Instance, frmScanControl.Instance.Right, 0);
                frmExObsCam.Instance.FrmLockFlg = 1;

                frmExObsCam.Instance.CaptureStart();
            }
        }

        public void ChangeSizeAndLocation(int DispFlg)
        {
            if (DispFlg == 0)
            {
                int theLeft = 0;
                int theTop = 0;
                //このモニタの幅と高さ（ピクセル値）を取得
                int ScreenWidth = 0;
                ScreenWidth = Winapi.GetSystemMetrics(Winapi.SM_CXSCREEN);
                int ScreenHeight = 0;
                ScreenHeight = Winapi.GetSystemMetrics(Winapi.SM_CYSCREEN);
                //仮想画面の左端の座標（ピクセル値）を取得
                int VirtualLeft = 0;
                VirtualLeft = Winapi.GetSystemMetrics(Winapi.SM_XVIRTUALSCREEN);

                //モニタの数が複数ではない
                if (Winapi.GetSystemMetrics(Winapi.SM_CMONITORS) < 2)
                {
                    frmExObsCam.Instance.Width = 512 + 6;
                    frmExObsCam.Instance.Height = 512 + 6;
                    //theLeft = ScreenWidth - (.width + 6) - 5
                    //変更2014/10/07hata_v19.51反映
                    //v17.4X/v18.00変更 by 間々田 2011/03/15 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
                    //theLeft = ScreenWidth - (_with3.Width + 6) - (frmTransImageControl.Instance.ClientRectangle.Width + 6);
                    //theTop = ScreenHeight - (_with3.Height + 25) - 95;
                    theLeft = frmScanImage.Instance.Left + frmScanImage.Instance.Width - frmExObsCam.Instance.Width;
                    theTop = frmScanImage.Instance.Top;

                    //モニタが複数：透視画像フォームは２ndモニターに移動
                    //仮想画面の左端の座標が負の場合
                }
                else if (VirtualLeft < 0)
                {
                    //Rev26.40 透視画面が隠れているとき by chouno 2019/02/20
                    if (frmTransImage.Instance.WindowState == FormWindowState.Minimized || frmTransImageControl.Instance.WindowState == FormWindowState.Minimized)
                    {
                        frmExObsCam.Instance.Width = 687;
                        frmExObsCam.Instance.Height = 710;
                        frmExObsCam.Instance.ClientSize = new Size(681,681);
                        theLeft = -ScreenWidth;  //左画面に右寄せ 'v17.10変更 byやまおか 2010/08/25
                        theTop = 0;
                    }
                    else
                    {
                        frmExObsCam.Instance.Width = ScreenWidth - (frmTransImage.Instance.Width + frmTransImageControl.Instance.Width);
                        frmExObsCam.Instance.Height = ScreenWidth - (frmTransImage.Instance.Width + frmTransImageControl.Instance.Width);
                        frmExObsCam.Instance.ClientSize = new Size(frmExObsCam.Instance.Width - 6, frmExObsCam.Instance.Width - 6);
                        //theLeft = VirtualLeft
                        theLeft = -(frmExObsCam.Instance.Width + frmTransImage.Instance.Width + frmTransImageControl.Instance.Width);  //左画面に右寄せ 'v17.10変更 byやまおか 2010/08/25
                        theTop = 0;
                    }

  
                }
                else
                {
                    theLeft = ScreenWidth;
                    theTop = 0;
                    frmExObsCam.Instance.Width = 512 + 6;
                    frmExObsCam.Instance.Height = 512 + 6;
                }

                //移動
                frmExObsCam.Instance.SetBounds(theLeft, theTop, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

                //フォームサイズ調整
                // Mod Start 2018/12/07 M.Oyama Windows10対応
                //frmExObsCam.Instance.Size = new Size(frmExObsCam.Instance.Width, frmExObsCam.Instance.Height);
                frmExObsCam.Instance.Size = new Size(frmExObsCam.Instance.Width + 15, frmExObsCam.Instance.Height + 15);
                // Mod End 2018/12/07
            }
            else if (DispFlg == 1)
            {
                frmExObsCam.Instance.Height = 1024 + frmExObsCam.Instance.Toolbar1.Height + 25;
                frmExObsCam.Instance.Width = 1030;
                frmExObsCam.Instance.ClientSize = new Size(frmScanImage.Instance.ClientSize.Width,frmScanImage.Instance.ClientSize.Height);
            }

        }

    }
}
