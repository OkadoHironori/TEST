using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CTAPI;
using CT30K.Common;
using CT30K;

namespace CommonViewer
{
    public partial class frmCommonViewer : Form
    {

        private static frmCommonViewer _Instance = null;

        //共有メモリのハンドル
        public static IntPtr hComMap = IntPtr.Zero;

        public frmCommonViewer()
        {
            InitializeComponent();
        }

        public static frmCommonViewer Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmCommonViewer();
                }

                return _Instance;
            }
        }

        //*******************************************************************************
        //機　　能： 「更新」ボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdUpdate_Click_1(System.Object eventSender, System.EventArgs eventArgs)
		{

			short i = 0;
			short j = 0;
			short K = 0;
            string[] tmpstr = null;
            //string tmpstr2 = null;
         
            //コモン読み込み
			modCT30K.GetCommon();

			var _with5 = CTSettings.t20kinf.Data;

			_txtT20kinf_0.Text = Convert.ToString(_with5.cpuready);
			_txtT20kinf_1.Text = Convert.ToString(_with5.scan_flag);
			_txtT20kinf_2.Text = _with5.system_name.GetString();
			_txtT20kinf_3.Text = _with5.system_type.GetString();
			_txtT20kinf_4.Text = _with5.version.GetString();
			_txtT20kinf_5.Text = _with5.ct_gentype.GetString();
			_txtT20kinf_6.Text = _with5.now_hdcopy.GetString();
			_txtT20kinf_7.Text = _with5.now_3ddisplay.GetString();
			//_txtT20kinf_8.Text = Convert.ToString(_with5.type_info);
            tmpstr = _with5.type_info.ToStrArray();
            _txtT20kinf_8.Text = tmpstr[0] + "\r\n" +
                                 tmpstr[1] + "\r\n" +
                                 tmpstr[2] + "\r\n" +
                                 tmpstr[3] + "\r\n" +
                                 tmpstr[4] + "\r\n" +
                                 tmpstr[5] + "\r\n" +
                                 tmpstr[6] + "\r\n" +
                                 tmpstr[7] + "\r\n" +
                                 tmpstr[8] + "\r\n" +
                                 tmpstr[9] + "\r\n" +
                                 tmpstr[10] + "\r\n" +
                                 tmpstr[11] + "\r\n" +
                                 tmpstr[12] + "\r\n" +
                                 tmpstr[13] + "\r\n" +
                                 tmpstr[14] + "\r\n" +
                                 tmpstr[15];
            _txtT20kinf_9.Text = Convert.ToString(_with5.upper_limit);
			_txtT20kinf_10.Text = Convert.ToString(_with5.lower_limit);
			_txtT20kinf_11.Text = Convert.ToString(_with5.ups_power);
			_txtT20kinf_12.Text = Convert.ToString(_with5.ud_type);
			_txtT20kinf_13.Text = Convert.ToString(_with5.v_capture_type);
			_txtT20kinf_14.Text = Convert.ToString(_with5.ct30k_running);
			_txtT20kinf_15.Text = _with5.instance_uid.GetString();
			_txtT20kinf_16.Text = _with5.macaddress.GetString();
			_txtT20kinf_17.Text = Convert.ToString(_with5.fx_lower_limit);
			_txtT20kinf_18.Text = Convert.ToString(_with5.fx_upper_limit);
			_txtT20kinf_19.Text = Convert.ToString(_with5.fy_lower_limit);
			_txtT20kinf_20.Text = Convert.ToString(_with5.fy_upper_limit);
			
            var _with6 = modDispinf.dispinf;

			_txtDispinf_0.Text = Convert.ToString(_with6.dpreq);
			_txtDispinf_1.Text = Convert.ToString(_with6.dpcomp);
			_txtDispinf_2.Text = Convert.ToString(_with6.imgdp);
			_txtDispinf_3.Text = _with6.d_exam;
			_txtDispinf_4.Text = _with6.d_id;
			_txtDispinf_5.Text = Convert.ToString(_with6.d_exammax);
			_txtDispinf_6.Text = Convert.ToString(_with6.d_idmax);
			_txtDispinf_7.Text = Convert.ToString(_with6.imgcnv);
			_txtDispinf_8.Text = Convert.ToString(_with6.winddp);
			_txtDispinf_9.Text = Convert.ToString(_with6.infdp);
			_txtDispinf_10.Text = Convert.ToString(_with6.roidp);
			_txtDispinf_11.Text = Convert.ToString(_with6.grpdp);
			_txtDispinf_12.Text = Convert.ToString(_with6.colormode);
			_txtDispinf_13.Text = Convert.ToString(_with6.wwwlmode);
			_txtDispinf_14.Text = Convert.ToString(_with6.Width);
			_txtDispinf_15.Text = Convert.ToString(_with6.level);
			_txtDispinf_16.Text = Convert.ToString(_with6.coloralpha);
			_txtDispinf_18.Text = Convert.ToString(_with6.Alpha);
			//txtDispinf[19].Text = modCommonViewer.LongArrayToStr(ref ref _with6.Rtbl);
			//txtDispinf[20].Text = modCommonViewer.LongArrayToStr(ref ref _with6.Gtbl);
			//txtDispinf[21].Text = modCommonViewer.LongArrayToStr(ref ref _with6.Btbl);
			_txtDispinf_22.Text = Convert.ToString(_with6.color_max);
			_txtDispinf_23.Text = Convert.ToString(_with6.color_min);
			
            var _with7 = modRoikey.roikey;

			_txtRoikey_0.Text = Convert.ToString(_with7.imgroi);
			_txtRoikey_1.Text = Convert.ToString(_with7.roi_mode);
			_txtRoikey_2.Text = Convert.ToString(_with7.roi_x);
			_txtRoikey_3.Text = Convert.ToString(_with7.roi_y);
			_txtRoikey_4.Text = Convert.ToString(_with7.roi_xsize);
			_txtRoikey_5.Text = Convert.ToString(_with7.roi_ysize);
			
            var _with8 = CTSettings.infdef.Data;

			lstXtable.Items.Clear();
            int tmp_cnt = 0;
			for (i = 0; i <= 2; i++) {
				for (j = 0; j <= 2; j++) {
					for (K = 0; K <= 1; K++) {
						//                    lstXtable.AddItem "x_table[" & CStr(i) & "][" & CStr(j) & "][" & CStr(K) & "] = " & RemoveNull(.x_table(K, j, i))
                        //lstXtable.AddItem "x_table[" + i.ToString() + "][" + j.ToString() + "][" + K.ToString() + "] = " + RemoveNull(_with8.x_table(K, j, i));
                        lstXtable.Items.Add("x_table[" + i.ToString() + "][" + j.ToString() + "][" + K.ToString() + "] = " + (CTSettings.xtable.Data.xtable[tmp_cnt]));
                        tmp_cnt++;
                    }
				}
			}
            tmp_cnt = 0;

            _txtInfdef_1.Text = _with8.focustype[0].GetString() + "\r\n" +
                                _with8.focustype[1].GetString() + "\r\n" +
                                _with8.focustype[2].GetString();
			//_txtInfdef_1.Text = _with8.focustype.ToString();
            //tmpstr = _with8.data_mode.ToStrArray();
            _txtInfdef_2.Text = _with8.data_mode[0].GetString() + "\r\n" +
                                _with8.data_mode[1].GetString() + "\r\n" +
                                _with8.data_mode[2].GetString();
            //_txtInfdef_2.Text = _with8.data_mode.ToString();
            //tmpstr = _with8.scan_mode.ToStrArray();
            _txtInfdef_3.Text = _with8.scan_mode[0].GetString() + "\r\n" +
                                _with8.scan_mode[1].GetString() + "\r\n" +
                                _with8.scan_mode[2].GetString();
            //_txtInfdef_3.Text = _with8.scan_mode.ToString();
            tmpstr = _with8.multiscan_mode.ToStrArray();
            _txtInfdef_4.Text = _with8.multiscan_mode[0].GetString() + "\r\n" +
                                _with8.multiscan_mode[1].GetString() + "\r\n" +
                                _with8.multiscan_mode[2].GetString();
            
            //_txtInfdef_4.Text = _with8.multiscan_mode.ToString();
			_txtInfdef_5.Text = _with8.auto_zoom.GetString();
			_txtInfdef_6.Text = _with8.auto_print.GetString();
			_txtInfdef_7.Text = _with8.raw_save.GetString();
			_txtInfdef_8.Text = _with8.bhc.GetString();
            //tmpstr = _with8.matrixsize.ToStrArray();
            _txtInfdef_9.Text = _with8.matrixsize[0].GetString() + "\r\n" +
                                _with8.matrixsize[1].GetString() + "\r\n" +
                                _with8.matrixsize[2].GetString() + "\r\n" +
                                _with8.matrixsize[3].GetString();

            //_txtInfdef_9.Text = _with8.matrixsize.ToString();
			//_txtInfdef_10.Text = _with8.scano_matrix.ToString();
            _txtInfdef_10.Text = _with8.scano_matrix[0].GetString() + "\r\n" +
                                 _with8.scano_matrix[1].GetString() + "\r\n" +
                                 _with8.scano_matrix[2].GetString() + "\r\n" +
                                 _with8.scano_matrix[3].GetString();
            
            //_txtInfdef_11.Text = _with8.scan_speed.ToString();
            _txtInfdef_11.Text = _with8.scan_speed[0].GetString() + "\r\n" +
                                 _with8.scan_speed[1].GetString() + "\r\n" +
                                 _with8.scan_speed[2].GetString();
            
            //_txtInfdef_12.Text = _with8.scano_speed.ToString();
            _txtInfdef_12.Text = _with8.scano_speed[0].GetString() + "\r\n" +
                                 _with8.scano_speed[1].GetString() + "\r\n" +
                                 _with8.scano_speed[2].GetString();
            
            //_txtInfdef_13.Text = _with8.scan_area.ToString();
            _txtInfdef_13.Text = _with8.scan_area[0].GetString() + "\r\n" +
                                 _with8.scan_area[1].GetString() + "\r\n" +
                                 _with8.scan_area[2].GetString();

			//_txtInfdef_14.Text = _with8.scano_area.ToString();
            _txtInfdef_14.Text = _with8.scano_area[0].GetString() + "\r\n" +
                                 _with8.scano_area[1].GetString() + "\r\n" +
                                 _with8.scano_area[2].GetString();           
            
            //_txtInfdef_15.Text = _with8.slice_wid.ToString();
            _txtInfdef_15.Text = _with8.slice_wid[0].GetString() + "\r\n" +
                                 _with8.slice_wid[1].GetString() + "\r\n" +
                                 _with8.slice_wid[2].GetString();

			//_txtInfdef_16.Text = _with8.scano_slice_wid.ToString();
            _txtInfdef_16.Text = _with8.scano_slice_wid[0].GetString() + "\r\n" +
                                 _with8.scano_slice_wid[0].GetString() + "\r\n" +
                                 _with8.scano_slice_wid[0].GetString();
            
            //_txtInfdef_17.Text = _with8.det_ap.ToString();
            _txtInfdef_17.Text = _with8.det_ap[0].GetString() + "\r\n" +
                                 _with8.det_ap[1].GetString() + "\r\n" +
                                 _with8.det_ap[2].GetString();
            
            //_txtInfdef_18.Text = _with8.fc.ToString();
            _txtInfdef_18.Text = _with8.fc[0].GetString() + "\r\n" +
                                 _with8.fc[1].GetString() + "\r\n" +
                                 _with8.fc[2].GetString();
            
            _txtInfdef_19.Text = _with8.max_view.GetString();
			_txtInfdef_20.Text = _with8.min_view.GetString();
			_txtInfdef_21.Text = _with8.max_integ_number.GetString();
			_txtInfdef_22.Text = _with8.min_integ_number.GetString();
			_txtInfdef_23.Text = _with8.fimage_bit.GetString();
			//_txtInfdef_24.Text = _with8.multislice.ToString();
            _txtInfdef_24.Text = _with8.multislice[0].GetString() + "\r\n" +
                                 _with8.multislice[0].GetString() + "\r\n" +
                                 _with8.multislice[0].GetString();                  
            
            //_txtInfdef_25.Text = _with8.multi_tube.ToString();
            _txtInfdef_25.Text = _with8.multi_tube[0].GetString() + "\r\n" +
                                 _with8.multi_tube[1].GetString();
            
            //_txtInfdef_26.Text = _with8.table_y.ToString();
            _txtInfdef_26.Text = _with8.table_y[0].GetString() + "\r\n" +
                                 _with8.table_y[1].GetString();

			//_txtInfdef_27.Text = _with8.detector.ToString();
            _txtInfdef_27.Text = _with8.detector[0].GetString() + "\r\n" +
                                 _with8.detector[1].GetString();
            
            //_txtInfdef_28.Text = _with8.iifield.ToString();
			_txtInfdef_28.Text = _with8.iifield[0].GetString() + "\r\n" +
                                 _with8.iifield[1].GetString() + "\r\n" +
                                 _with8.iifield[2].GetString();
                                  
            var _with9 = CTSettings.ctinfdef.Data;

			//_txtCtinfdef_0.Text = _with9.d_rawsts.ToString();
            _txtCtinfdef_0.Text = _with9.d_rawsts[0].GetString() + "\r\n" +
                                  _with9.d_rawsts[1].GetString() + "\r\n" +
                                  _with9.d_rawsts[2].GetString() + "\r\n" +
                                  _with9.d_rawsts[3].GetString() + "\r\n" +
                                  _with9.d_rawsts[4].GetString() + "\r\n" +
                                  _with9.d_rawsts[5].GetString() + "\r\n" +
                                  _with9.d_rawsts[6].GetString() + "\r\n" +
                                  _with9.d_rawsts[7].GetString() + "\r\n" +
                                  _with9.d_rawsts[8].GetString();

			//_txtCtinfdef_1.Text = _with9.d_recokind.ToString();
            _txtCtinfdef_1.Text = _with9.d_recokind[0].GetString() + "\r\n" +
                                  _with9.d_recokind[1].GetString() + "\r\n" +
                                  _with9.d_recokind[2].GetString();         
            
            //_txtCtinfdef_2.Text = _with9.full_mode.ToString();
            _txtCtinfdef_2.Text = _with9.full_mode[0].GetString() + "\r\n" +
                                  _with9.full_mode[1].GetString() + "\r\n" +
                                  _with9.full_mode[2].GetString();

			//_txtCtinfdef_3.Text = _with9.matsiz.ToString();
            _txtCtinfdef_3.Text = _with9.matsiz[0].GetString() + "\r\n" +
                                  _with9.matsiz[1].GetString() + "\r\n" +
                                  _with9.matsiz[2].GetString() + "\r\n" +
                                  _with9.matsiz[3].GetString() + "\r\n" +
                                  _with9.matsiz[4].GetString();

			//_txtCtinfdef_4.Text = _with9.scan_mode.ToString();
            _txtCtinfdef_4.Text = _with9.scan_mode[0].GetString() + "\r\n" +
                                  _with9.scan_mode[1].GetString() + "\r\n" +
                                  _with9.scan_mode[2].GetString() + "\r\n" +
                                  _with9.scan_mode[3].GetString();
            
            //_txtCtinfdef_5.Text = _with9.scan_speed.ToString();
            _txtCtinfdef_5.Text = _with9.scan_speed[0].GetString() + "\r\n" +
                                  _with9.scan_speed[1].GetString() + "\r\n" +
                                  _with9.scan_speed[2].GetString();
            
            //_txtCtinfdef_6.Text = _with9.scan_time.ToString();
            _txtCtinfdef_6.Text = _with9.scan_time[0].GetString() + "\r\n" +
                                  _with9.scan_time[1].GetString() + "\r\n" +
                                  _with9.scan_time[2].GetString();
                              
            //_txtCtinfdef_7.Text = _with9.scan_area.ToString();
            _txtCtinfdef_7.Text = _with9.scan_area[0].GetString() + "\r\n" +
                                  _with9.scan_area[1].GetString() + "\r\n" +
                                  _with9.scan_area[2].GetString();

			//_txtCtinfdef_8.Text = _with9.slice_wid.ToString();
            _txtCtinfdef_8.Text = _with9.slice_wid[0].GetString() + "\r\n" +
                                  _with9.slice_wid[1].GetString() + "\r\n" +
                                  _with9.slice_wid[2].GetString();
                          
			//_txtCtinfdef_9.Text = _with9.det_ap.ToString();
            _txtCtinfdef_9.Text = _with9.det_ap[0].GetString() + "\r\n" +
                                  _with9.det_ap[1].GetString() + "\r\n" +
                                  _with9.det_ap[2].GetString();

			//_txtCtinfdef_10.Text = _with9.focus.ToString();
            _txtCtinfdef_10.Text = _with9.focus[0].GetString() + "\r\n" +
                                   _with9.focus[1].GetString() + "\r\n" +
                                   _with9.focus[2].GetString();
            
            //_txtCtinfdef_11.Text = _with9.focustype.ToString();
            _txtCtinfdef_11.Text = _with9.focustype[0].GetString() + "\r\n" +
                                   _with9.focustype[1].GetString() + "\r\n" +
                                   _with9.focustype[2].GetString() + "\r\n" +
                                   _with9.focustype[3].GetString() + "\r\n" +
                                   _with9.focustype[4].GetString() + "\r\n" +
                                   _with9.focustype[5].GetString();

			//_txtCtinfdef_12.Text = _with9.energy.ToString();
            _txtCtinfdef_12.Text = _with9.energy[0].GetString() + "\r\n" +
                                   _with9.energy[1].GetString() + "\r\n" +
                                   _with9.energy[2].GetString();

			//_txtCtinfdef_13.Text = _with9.tilt_angle.ToString();
            _txtCtinfdef_13.Text = _with9.tilt_angle[0].GetString() + "\r\n" +
                                   _with9.tilt_angle[1].GetString() + "\r\n" +
                                   _with9.tilt_angle[2].GetString();
            
            //_txtCtinfdef_14.Text = _with9.fc.ToString();
            _txtCtinfdef_14.Text = _with9.fc[0].GetString() + "\r\n" +
                                   _with9.fc[1].GetString() + "\r\n" +
                                   _with9.fc[2].GetString();

			//_txtCtinfdef_15.Text = _with9.sift_pos.ToString();
            _txtCtinfdef_15.Text = _with9.sift_pos[0].GetString() + "\r\n" +
                                   _with9.sift_pos[1].GetString() + "\r\n" +
                                   _with9.sift_pos[2].GetString() + "\r\n" +
                                   _with9.sift_pos[3].GetString() + "\r\n" +
                                   _with9.sift_pos[4].GetString();

            //_txtCtinfdef_16.Text = _with9.scano_dir.ToString();
            _txtCtinfdef_16.Text = _with9.scano_dir[0].GetString() + "\r\n" +
                                   _with9.scano_dir[1].GetString() + "\r\n" +
                                   _with9.scano_dir[2].GetString();			
            
            //_txtCtinfdef_17.Text = _with9.pro_dir.ToString();
            _txtCtinfdef_17.Text = _with9.pro_dir[0].GetString() + "\r\n" +
                                   _with9.pro_dir[1].GetString() + "\r\n" +
                                   _with9.pro_dir[2].GetString();		

			//_txtCtinfdef_18.Text = _with9.view_dir.ToString();
            _txtCtinfdef_18.Text = _with9.view_dir[0].GetString() + "\r\n" +
                                   _with9.view_dir[1].GetString() + "\r\n" +
                                   _with9.view_dir[2].GetString();					
            
            //_txtCtinfdef_19.Text = _with9.pro_posdir.ToString();
            _txtCtinfdef_19.Text = _with9.pro_posdir[0].GetString() + "\r\n" +
                                   _with9.pro_posdir[1].GetString() + "\r\n" +
                                   _with9.pro_posdir[2].GetString() + "\r\n" +
                                   _with9.pro_posdir[3].GetString() + "\r\n" +
                                   _with9.pro_posdir[4].GetString();

			//_txtCtinfdef_20.Text = _with9.scano_dispdir.ToString();
            _txtCtinfdef_20.Text = _with9.scano_dispdir.GetString();

			//_txtCtinfdef_21.Text = _with9.rotation.ToString();
            _txtCtinfdef_21.Text = _with9.rotation[0].GetString() + "\r\n" +
                                   _with9.rotation[1].GetString() + "\r\n" +
                                   _with9.rotation[2].GetString() + "\r\n" +
                                   _with9.rotation[3].GetString();

			_txtCtinfdef_22.Text = _with9.w_lamp_size.GetString();
			
            //_txtCtinfdef_23.Text = _with9.imgsize_2.ToString();
            _txtCtinfdef_21.Text = _with9.imgsize_2[0].GetString() + "\r\n" +
                                   _with9.imgsize_2[1].GetString() + "\r\n" +
                                   _with9.imgsize_2[2].GetString() + "\r\n" +
                                   _with9.imgsize_2[3].GetString();

			//_txtCtinfdef_24.Text = _with9.scano_matsiz.ToString();
            _txtCtinfdef_21.Text = _with9.scano_matsiz[0].GetString() + "\r\n" +
                                   _with9.scano_matsiz[1].GetString() + "\r\n" +
                                   _with9.scano_matsiz[2].GetString() + "\r\n" +
                                   _with9.scano_matsiz[3].GetString();

            var _with10 = CTSettings.scancondpar.Data;;

            //_txtScancondpar_2.Text = _with10.realfdd.ToString();
            tmpstr = _with10.realfdd.ToStrArray();
            _txtScancondpar_2.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];

			//_txtScancondpar_4.Text = _with10.fanangle.ToString();
            tmpstr = _with10.fanangle.ToStrArray();
            _txtScancondpar_4.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
			
            
            //_txtScancondpar_5.Text = _with10.mainch.ToString();
            tmpstr = _with10.mainch.ToStrArray();
            _txtScancondpar_5.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
			
            //_txtScancondpar_6.Text = _with10.refch.ToString();
            tmpstr = _with10.refch.ToStrArray();
            _txtScancondpar_6.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];

            //_txtScancondpar_7.Text = _with10.dtunitno.ToString();
            tmpstr = _with10.dtunitno.ToStrArray();
            _txtScancondpar_7.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_8.Text = _with10.dtunitch.ToString();
            tmpstr = _with10.dtunitch.ToStrArray();
            _txtScancondpar_8.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            
            //_txtScancondpar_9.Text = _with10.dtpitch.ToString();
            tmpstr = _with10.dtpitch.ToStrArray();
            _txtScancondpar_9.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_10.Text = _with10.trvsno.ToString();
            tmpstr = _with10.trvsno.ToStrArray();
            _txtScancondpar_10.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];

			//_txtScancondpar_13.Text = _with10.dmych.ToString();
            tmpstr = _with10.dmych.ToStrArray();
            _txtScancondpar_13.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_14.Text = _with10.offsetapr.ToString();
            tmpstr = _with10.offsetapr.ToStrArray();
            _txtScancondpar_14.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_15.Text =_with10.offsetpt.ToString();
            tmpstr = _with10.offsetpt.ToStrArray();
            _txtScancondpar_15.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_16.Text =_with10.airdtpt.ToString();
            tmpstr = _with10.airdtpt.ToStrArray();
            _txtScancondpar_16.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];

			//_txtScancondpar_21.Text = _with10.scanotvs.ToString();
            tmpstr = _with10.scanotvs.ToStrArray();
            _txtScancondpar_21.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];			
            
            //_txtScancondpar_22.Text = _with10.scanoapr.ToString();
            tmpstr = _with10.scanoapr.ToStrArray();
            _txtScancondpar_22.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_23.Text = _with10.scanopt.ToString();
            tmpstr = _with10.scanopt.ToStrArray();
            _txtScancondpar_23.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_24.Text = _with10.scanoscl.ToString();
            tmpstr = _with10.scanoscl.ToStrArray();
            _txtScancondpar_24.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_25.Text = _with10.scanoair.ToString();
            tmpstr = _with10.scanoair.ToStrArray();
            _txtScancondpar_25.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];

			//_txtScancondpar_32.Text = _with10.scan_stroke.ToString();
            tmpstr = _with10.scan_stroke.ToStrArray();
            _txtScancondpar_5.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];

            //_txtScancondpar_33.Text = _with10.scano_dp_ratio.ToString();
            tmpstr = _with10.scano_dp_ratio.ToStrArray();
            _txtScancondpar_33.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];

            _txtScancondpar_34.Text = Convert.ToString(_with10.fimage_hsize);
           
            _txtScancondpar_35.Text = Convert.ToString(_with10.fimage_vsize);

			//_txtScancondpar_37.Text = _with10.xls.ToString();
            tmpstr = _with10.xls.ToStrArray();
            _txtScancondpar_37.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4];
 
			//_txtScancondpar_38.Text = _with10.xle.ToString();
            tmpstr = _with10.xle.ToStrArray();
            _txtScancondpar_38.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4];			
            
            
            
            //_txtScancondpar_39.Text = _with10.xlc.ToString();
            tmpstr = _with10.xlc.ToStrArray();
            _txtScancondpar_39.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4];

			//_txtScancondpar_40.Text = _with10.scan_posi_a.ToString();
            tmpstr = _with10.scan_posi_a.ToStrArray();
            _txtScancondpar_40.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4];

			//_txtScancondpar_41.Text = _with10.scan_posi_b.ToString();
            tmpstr = _with10.scan_posi_b.ToStrArray();
            _txtScancondpar_41.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4];

            _txtScancondpar_42.Text = Convert.ToString(_with10.fimage_bit);

			_txtScancondpar_47.Text = Convert.ToString(_with10.recon_start_angle);
			
            //_txtScancondpar_48.Text = _with10.mdtpitch.ToString();
            tmpstr = _with10.mdtpitch.ToStrArray();
            _txtScancondpar_48.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4];
            
            //_txtScancondpar_49.Text = _with10.fid_offset.ToString();
            tmpstr = _with10.xls.ToStrArray();
            _txtScancondpar_49.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1];
                                  
            //_txtScancondpar_50.Text = _with10.fcd_offset.ToString();
            tmpstr = _with10.fcd_offset.ToStrArray();
            _txtScancondpar_50.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1];
 
			//_txtScancondpar_52.Text = _with10.v_mag.ToString();
            tmpstr = _with10.v_mag.ToStrArray();
            _txtScancondpar_52.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            _txtScancondpar_53.Text = Convert.ToString(_with10.ver_wire_pitch);
			
            //_txtScancondpar_54.Text = _with10.b.ToString();
            tmpstr = _with10.b.ToStrArray();
            _txtScancondpar_54.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4] + "\r\n" +
                                     tmpstr[5];
            
            //_txtScancondpar_55.Text = _with10.n1.ToString();
            tmpstr = _with10.n1.ToStrArray();
            _txtScancondpar_55.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1];
            
            //_txtScancondpar_56.Text = _with10.n2.ToString();
            tmpstr = _with10.n2.ToStrArray();
            _txtScancondpar_56.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1];
                             
            _txtScancondpar_57.Text = Convert.ToString(_with10.delta_theta);
			_txtScancondpar_58.Text = Convert.ToString(_with10.dpm);
			_txtScancondpar_59.Text = Convert.ToString(_with10.n0);
			_txtScancondpar_60.Text = Convert.ToString(_with10.m0);
			
            //_txtScancondpar_61.Text = _with10.theta0.ToString();
            tmpstr = _with10.theta0.ToStrArray();
            _txtScancondpar_61.Text = tmpstr[0] + "\r\n" +
                                      tmpstr[1];

            //_txtScancondpar_62.Text = _with10.theta01.ToString();
            tmpstr = _with10.theta01.ToStrArray();
            _txtScancondpar_62.Text = tmpstr[0] + "\r\n" +
                                      tmpstr[1];
                               
            //_txtScancondpar_63.Text = _with10.theta02.ToString();
            tmpstr = _with10.xls.ToStrArray();
            _txtScancondpar_63.Text = tmpstr[0] + "\r\n" +
                                      tmpstr[1];
 
            _txtScancondpar_64.Text = Convert.ToString(_with10.thetaoff);
			_txtScancondpar_65.Text = Convert.ToString(_with10.ioff);
			_txtScancondpar_66.Text = Convert.ToString(_with10.alpha);
			_txtScancondpar_67.Text = Convert.ToString(_with10.nc);
			_txtScancondpar_68.Text = Convert.ToString(_with10.klimit);
			_txtScancondpar_69.Text = Convert.ToString(_with10.fcd_limit);
			_txtScancondpar_70.Text = Convert.ToString(_with10.fud_limit);
			_txtScancondpar_71.Text = Convert.ToString(_with10.fud_step);
			_txtScancondpar_72.Text = Convert.ToString(_with10.rud_step);
			_txtScancondpar_73.Text = Convert.ToString(_with10.fud_start);
			_txtScancondpar_74.Text = Convert.ToString(_with10.fud_end);
			_txtScancondpar_75.Text = Convert.ToString(_with10.rud_start);
			_txtScancondpar_76.Text = Convert.ToString(_with10.rud_end);
			_txtScancondpar_77.Text = Convert.ToString(_with10.y_incli);
			_txtScancondpar_78.Text = Convert.ToString(_with10.xposition);
			_txtScancondpar_79.Text = Convert.ToString(_with10.study_id);
			_txtScancondpar_80.Text = Convert.ToString(_with10.acq_num);
			_txtScancondpar_81.Text = Convert.ToString(_with10.series_num);
			_txtScancondpar_82.Text = Convert.ToString(_with10.scan_comp);
			_txtScancondpar_83.Text = Convert.ToString(_with10.fs);
			_txtScancondpar_84.Text = Convert.ToString(_with10.x0);
			_txtScancondpar_85.Text = Convert.ToString(_with10.x1);
			_txtScancondpar_86.Text = Convert.ToString(_with10.x2);
			_txtScancondpar_87.Text = Convert.ToString(_with10.x3);
			_txtScancondpar_88.Text = Convert.ToString(_with10.x4);
			_txtScancondpar_89.Text = Convert.ToString(_with10.x_offset);
			_txtScancondpar_90.Text = Convert.ToString(_with10.ref_fid);
			_txtScancondpar_91.Text = Convert.ToString(_with10.detector);

			//_txtScancondpar_93.Text = _with10.h_mag.ToString();
            tmpstr = _with10.h_mag.ToStrArray();
            _txtScancondpar_93.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];

			//_txtScancondpar_94.Text = _with10.pulsar_v_mag.ToString();
            tmpstr = _with10.pulsar_v_mag.ToStrArray();
            _txtScancondpar_94.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
                                     
			//_txtScancondpar_95.Text = _with10.pulsar_h_mag.ToString();
            tmpstr = _with10.pulsar_h_mag.ToStrArray();
            _txtScancondpar_95.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
            
            //_txtScancondpar_96.Text = _with10.fpulsar_v_mag.ToString();
            tmpstr = _with10.fpulsar_v_mag.ToStrArray();
            _txtScancondpar_96.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
          

			//_txtScancondpar_97.Text = _with10.fpulsar_h_mag.ToString();
            tmpstr = _with10.fpulsar_h_mag.ToStrArray();
            _txtScancondpar_97.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
              
			_txtScancondpar_98.Text = Convert.ToString(_with10.fpd_pitch);
			_txtScancondpar_99.Text = Convert.ToString(_with10.rot_max_speed);
			_txtScancondpar_100.Text = Convert.ToString(_with10.scan_start_angle);
			_txtScancondpar_101.Text = Convert.ToString(_with10.rc_slope_ft);
			_txtScancondpar_102.Text = Convert.ToString(_with10.dc_center_ft_x);
			_txtScancondpar_103.Text = Convert.ToString(_with10.dc_center_ft_y);
			_txtScancondpar_104.Text = Convert.ToString(_with10.h_size);
			_txtScancondpar_105.Text = Convert.ToString(_with10.v_size);
			_txtScancondpar_106.Text = Convert.ToString(_with10.ftable_max_speed);
			_txtScancondpar_107.Text = Convert.ToString(_with10.ud_max_speed);
			_txtScancondpar_108.Text = _with10.distribute_drive.GetString();
			_txtScancondpar_109.Text = Convert.ToString(_with10.distribute_pc_no);
			_txtScancondpar_110.Text = Convert.ToString(_with10.xrot_start_pos);
			_txtScancondpar_111.Text = Convert.ToString(_with10.xrot_end_pos);
			_txtScancondpar_112.Text = Convert.ToString(_with10.magnify_para);
			_txtScancondpar_113.Text = Convert.ToString(_with10.recon_offset_angle);
			_txtScancondpar_114.Text = Convert.ToString(_with10.rnw1);
			_txtScancondpar_115.Text = Convert.ToString(_with10.rnw2);
			_txtScancondpar_116.Text = Convert.ToString(_with10.alpha_h);

			//_txtScancondpar_117.Text = _with10.max_mfanangle.ToString();
            tmpstr = _with10.max_mfanangle.ToStrArray();
            _txtScancondpar_117.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4];

            _txtScancondpar_118.Text = Convert.ToString(_with10.cone_max_mfanangle);
			_txtScancondpar_119.Text = Convert.ToString(_with10.dev_klimit);
			
            //_txtScancondpar_120.Text = _with10.detector_pitch.ToString();
            tmpstr = _with10.detector_pitch.ToStrArray();
            _txtScancondpar_120.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2];
                                   
            _txtScancondpar_121.Text = Convert.ToString(_with10.noise_cpc);
			//_txtScancondpar_122.Text = _with10.alk.ToString();
            tmpstr = _with10.alk.ToStrArray();
            _txtScancondpar_122.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4] + "\r\n" +
                                     tmpstr[5] + "\r\n" +
                                     tmpstr[6] + "\r\n" +
                                     tmpstr[7] + "\r\n" +
                                     tmpstr[8] + "\r\n" +
                                     tmpstr[9] + "\r\n" +
                                     tmpstr[10] + "\r\n" +
                                     tmpstr[11] + "\r\n" +
                                     tmpstr[12] + "\r\n" +
                                     tmpstr[13] + "\r\n" +
                                     tmpstr[14] + "\r\n" +
                                     tmpstr[15] + "\r\n" +
                                     tmpstr[16] + "\r\n" +
                                     tmpstr[17] + "\r\n" +
                                     tmpstr[18] + "\r\n" +
                                     tmpstr[19] + "\r\n" +
                                     tmpstr[20] + "\r\n" +
                                     tmpstr[21] + "\r\n" +
                                     tmpstr[22] + "\r\n" +
                                     tmpstr[23] + "\r\n" +
                                     tmpstr[24] + "\r\n" +
                                     tmpstr[25] + "\r\n" +
                                     tmpstr[26] + "\r\n" +
                                     tmpstr[27] + "\r\n" +
                                     tmpstr[28] + "\r\n" +
                                     tmpstr[29] + "\r\n" +
                                     tmpstr[30] + "\r\n" +
                                     tmpstr[31] + "\r\n" +
                                     tmpstr[32] + "\r\n" +
                                     tmpstr[33] + "\r\n" +
                                     tmpstr[34] + "\r\n" +
                                     tmpstr[35];

			//_txtScancondpar_123.Text = _with10.blk.ToString();
            tmpstr = _with10.blk.ToStrArray();
            _txtScancondpar_123.Text = tmpstr[0] + "\r\n" +
                                     tmpstr[1] + "\r\n" +
                                     tmpstr[2] + "\r\n" +
                                     tmpstr[3] + "\r\n" +
                                     tmpstr[4] + "\r\n" +
                                     tmpstr[5] + "\r\n" +
                                     tmpstr[6] + "\r\n" +
                                     tmpstr[7] + "\r\n" +
                                     tmpstr[8] + "\r\n" +
                                     tmpstr[9] + "\r\n" +
                                     tmpstr[10] + "\r\n" +
                                     tmpstr[11] + "\r\n" +
                                     tmpstr[12] + "\r\n" +
                                     tmpstr[13] + "\r\n" +
                                     tmpstr[14] + "\r\n" +
                                     tmpstr[15] + "\r\n" +
                                     tmpstr[16] + "\r\n" +
                                     tmpstr[17] + "\r\n" +
                                     tmpstr[18] + "\r\n" +
                                     tmpstr[19] + "\r\n" +
                                     tmpstr[20] + "\r\n" +
                                     tmpstr[21] + "\r\n" +
                                     tmpstr[22] + "\r\n" +
                                     tmpstr[23] + "\r\n" +
                                     tmpstr[24] + "\r\n" +
                                     tmpstr[25] + "\r\n" +
                                     tmpstr[26] + "\r\n" +
                                     tmpstr[27] + "\r\n" +
                                     tmpstr[28] + "\r\n" +
                                     tmpstr[29] + "\r\n" +
                                     tmpstr[30] + "\r\n" +
                                     tmpstr[31] + "\r\n" +
                                     tmpstr[32] + "\r\n" +
                                     tmpstr[33] + "\r\n" +
                                     tmpstr[34] + "\r\n" +
                                     tmpstr[35];

			_txtScancondpar_124.Text = Convert.ToString(_with10.ist);
			_txtScancondpar_125.Text = Convert.ToString(_with10.ied);
			_txtScancondpar_126.Text = Convert.ToString(_with10.jst);
			_txtScancondpar_127.Text = Convert.ToString(_with10.jed);
			_txtScancondpar_128.Text = Convert.ToString(_with10.cone_scan_posi_a);
			_txtScancondpar_129.Text = Convert.ToString(_with10.cone_scan_posi_b);
			_txtScancondpar_130.Text = Convert.ToString(_with10.mbhc_airLogValue);
			
            var _with11 = CTSettings.scaninh.Data;

			//_txtScaninh_0.Text = _with11.data_mode.ToString();
            tmpstr = _with11.data_mode.ToStrArray();
            _txtScaninh_0.Text = tmpstr[0] + "\r\n" +
                                 tmpstr[1] + "\r\n" +
                                 tmpstr[2] + "\r\n" +
                                 tmpstr[3];

			//_txtScaninh_1.Text = _with11.scan_mode.ToString();
            tmpstr = _with11.scan_mode.ToStrArray();
            _txtScaninh_1.Text = tmpstr[0] + "\r\n" +
                                 tmpstr[1] + "\r\n" +
                                 tmpstr[2];

            //_txtScaninh_2.Text = _with11.multiscan_mode.ToString();
            tmpstr = _with11.multiscan_mode.ToStrArray();
            _txtScaninh_2.Text = tmpstr[0] + "\r\n" +
                                 tmpstr[1] + "\r\n" +
                                 tmpstr[2];
            
            _txtScaninh_3.Text = Convert.ToString(_with11.auto_zoom);
			_txtScaninh_4.Text = Convert.ToString(_with11.auto_print);
			_txtScaninh_5.Text = Convert.ToString(_with11.bhc);
			_txtScaninh_6.Text = Convert.ToString(_with11.raw_save);
			
            //_txtScaninh_7.Text = _with11.scan_matrix.ToString();
            tmpstr = _with11.scan_matrix.ToStrArray();
            _txtScaninh_7.Text = tmpstr[0] + "\r\n" +
                                 tmpstr[1] + "\r\n" +
                                 tmpstr[2] + "\r\n" +
                                 tmpstr[3];
            
            //_txtScaninh_8.Text = _with11.scan_speed.ToString();
            tmpstr = _with11.scan_speed.ToStrArray();
            _txtScaninh_8.Text = tmpstr[0] + "\r\n" +
                                 tmpstr[1] + "\r\n" +
                                 tmpstr[2];
            
            //_txtScaninh_9.Text = _with11.scan_area.ToString();
            tmpstr = _with11.scan_area.ToStrArray();
            _txtScaninh_9.Text = tmpstr[0] + "\r\n" +
                                 tmpstr[1] + "\r\n" +
                                 tmpstr[2];

			//_txtScaninh_10.Text = _with11.scan_det_ap.ToString();
            tmpstr = _with11.scan_det_ap.ToStrArray();
            _txtScaninh_10.Text = tmpstr[0] + "\r\n" +
                                 tmpstr[1] + "\r\n" +
                                 tmpstr[2];
            
            //_txtScaninh_11.Text = _with11.scan_width.ToString();
            tmpstr = _with11.scan_width.ToStrArray();
            _txtScaninh_11.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];
            
            //_txtScaninh_12.Text = _with11.scan_filter.ToString();
            tmpstr = _with11.scan_filter.ToStrArray();
            _txtScaninh_12.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

            //_txtScaninh_13.Text = _with11.scan_energy.ToString();
            tmpstr = _with11.scan_energy.ToStrArray();
            _txtScaninh_13.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

            //_txtScaninh_14.Text = _with11.scan_focus.ToString();
            tmpstr = _with11.scan_focus.ToStrArray();
            _txtScaninh_14.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];
            
            //_txtScaninh_15.Text = _with11.scano_matrix.ToString();
            tmpstr = _with11.scano_matrix.ToStrArray();
            _txtScaninh_15.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2] + "\r\n" +
                                  tmpstr[3];
            
            //_txtScaninh_16.Text = _with11.scano_speed.ToString();
            tmpstr = _with11.scano_speed.ToStrArray();
            _txtScaninh_11.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

            //_txtScaninh_17.Text = _with11.scano_area.ToString();
            tmpstr = _with11.scano_area.ToStrArray();
            _txtScaninh_17.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

            //_txtScaninh_18.Text = _with11.scano_det_ap.ToString();
            tmpstr = _with11.scano_det_ap.ToStrArray();
            _txtScaninh_11.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

            //_txtScaninh_19.Text = _with11.scano_width.ToString();
            tmpstr = _with11.scano_width.ToStrArray();
            _txtScaninh_19.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

			//_txtScaninh_20.Text = _with11.scano_energy.ToString();
            tmpstr = _with11.scano_energy.ToStrArray();
            _txtScaninh_20.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

			//_txtScaninh_21.Text = _with11.scano_focus.ToString();
            tmpstr = _with11.scano_focus.ToStrArray();
            _txtScaninh_21.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];
            
            _txtScaninh_22.Text = Convert.ToString(_with11.focus_change);
			_txtScaninh_23.Text = Convert.ToString(_with11.shutterfilter);
			_txtScaninh_24.Text = Convert.ToString(_with11.ud_mecha_pres);
			_txtScaninh_25.Text = Convert.ToString(_with11.multislice);
			_txtScaninh_26.Text = Convert.ToString(_with11.multi_tube);
			_txtScaninh_27.Text = Convert.ToString(_with11.xray_remote);
			_txtScaninh_28.Text = Convert.ToString(_with11.seqcomm);
			_txtScaninh_29.Text = Convert.ToString(_with11.auto_centering);
			_txtScaninh_30.Text = Convert.ToString(_with11.cor_status);
			_txtScaninh_31.Text = Convert.ToString(_with11.auto_cor);
			_txtScaninh_32.Text = Convert.ToString(_with11.tilt);
			_txtScaninh_33.Text = Convert.ToString(_with11.iifield);
			_txtScaninh_34.Text = Convert.ToString(_with11.collimator);
			_txtScaninh_35.Text = Convert.ToString(_with11.filter);
			_txtScaninh_36.Text = Convert.ToString(_with11.fine_table);
			_txtScaninh_37.Text = Convert.ToString(_with11.slice_light);
			_txtScaninh_38.Text = Convert.ToString(_with11.table_auto_move);
			_txtScaninh_39.Text = Convert.ToString(_with11.scan_wizard);
			_txtScaninh_40.Text = Convert.ToString(_with11.mechacontrol);
			_txtScaninh_41.Text = Convert.ToString(_with11.helical);
			_txtScaninh_42.Text = Convert.ToString(_with11.ext_trig);
			_txtScaninh_43.Text = Convert.ToString(_with11.table_down_acquire);
			_txtScaninh_44.Text = Convert.ToString(_with11.binning);
			_txtScaninh_45.Text = Convert.ToString(_with11.mecha_ref_ac);
			_txtScaninh_46.Text = Convert.ToString(_with11.table_y);
			_txtScaninh_47.Text = Convert.ToString(_with11.table_x);

			//_txtScaninh_48.Text = _with11.bin_char.ToString();
            tmpstr = _with11.bin_char.ToStrArray();
            _txtScaninh_48.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

			//_txtScaninh_49.Text = _with11.iifield_char.ToString();
            tmpstr = _with11.iifield_char.ToStrArray();
            _txtScaninh_49.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

			_txtScaninh_50.Text = Convert.ToString(_with11.collimator_ud);
			_txtScaninh_51.Text = Convert.ToString(_with11.collimator_rl);
			_txtScaninh_52.Text = Convert.ToString(_with11.fpd_frame);
			_txtScaninh_53.Text = Convert.ToString(_with11.fine_table_x);
			_txtScaninh_54.Text = Convert.ToString(_with11.fine_table_y);
			_txtScaninh_55.Text = Convert.ToString(_with11.scan_posi_entry_auto);

			//_txtScaninh_56.Text = _with11.cone_multiscan_mode.ToString();
            tmpstr = _with11.cone_multiscan_mode.ToStrArray();
            _txtScaninh_56.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];
            
            _txtScaninh_57.Text = Convert.ToString(_with11.cone_distribute);
			_txtScaninh_58.Text = Convert.ToString(_with11.rotate_select);
			_txtScaninh_59.Text = Convert.ToString(_with11.round_trip);
			_txtScaninh_60.Text = Convert.ToString(_with11.over_scan);
			_txtScaninh_61.Text = Convert.ToString(_with11.xray_rotate);
			_txtScaninh_62.Text = Convert.ToString(_with11.mail_send);
			_txtScaninh_63.Text = Convert.ToString(_with11.discharge_protect);
			_txtScaninh_64.Text = Convert.ToString(_with11.pc_freeze);
			_txtScaninh_65.Text = Convert.ToString(_with11.table_restriction);
			_txtScaninh_66.Text = Convert.ToString(_with11.ii_move);
			_txtScaninh_67.Text = Convert.ToString(_with11.artifact_reduction);
			_txtScaninh_68.Text = Convert.ToString(_with11.post_cone_reconstruction);
			_txtScaninh_69.Text = Convert.ToString(_with11.pcws2);
			_txtScaninh_70.Text = Convert.ToString(_with11.pcws3);
			_txtScaninh_71.Text = Convert.ToString(_with11.pcws4);
			_txtScaninh_72.Text = Convert.ToString(_with11.cone_distribute2);
			_txtScaninh_73.Text = Convert.ToString(_with11.full_distortion);
			_txtScaninh_74.Text = Convert.ToString(_with11.door_lock);
			_txtScaninh_75.Text = Convert.ToString(_with11.door_keyinput);
			
            var _with12 = CTSettings.scansel.Data;

			_txtScansel_0.Text = Convert.ToString(_with12.data_mode);
			_txtScansel_1.Text = Convert.ToString(_with12.scan_focus);
			_txtScansel_2.Text = Convert.ToString(_with12.scan_energy);
			_txtScansel_3.Text = Convert.ToString(_with12.scan_width);
			_txtScansel_4.Text = Convert.ToString(_with12.scan_mode);
			_txtScansel_5.Text = Convert.ToString(_with12.multiscan_mode);
			_txtScansel_6.Text = Convert.ToString(_with12.scan_speed);
			_txtScansel_7.Text = Convert.ToString(_with12.scan_area);
			_txtScansel_8.Text = Convert.ToString(_with12.filter);
			_txtScansel_9.Text = Convert.ToString(_with12.bhc_flag);
			_txtScansel_10.Text = _with12.bhc_dir.GetString();
			_txtScansel_11.Text = _with12.bhc_name.GetString();
			_txtScansel_12.Text = Convert.ToString(_with12.pitch);
			_txtScansel_13.Text = Convert.ToString(_with12.multinum);
			_txtScansel_14.Text = Convert.ToString(_with12.matrix_size);
			_txtScansel_15.Text = Convert.ToString(_with12.det_ap);
			_txtScansel_16.Text = Convert.ToString(_with12.tilt_angle);
			_txtScansel_17.Text = Convert.ToString(_with12.scano_focus);
			_txtScansel_18.Text = Convert.ToString(_with12.scano_energy);
			_txtScansel_19.Text = Convert.ToString(_with12.scano_width);
			_txtScansel_20.Text = Convert.ToString(_with12.scano_speed);
			_txtScansel_21.Text = Convert.ToString(_with12.scano_area);
			_txtScansel_22.Text = Convert.ToString(_with12.scano_matrix_size);
			_txtScansel_23.Text = Convert.ToString(_with12.scano_det_ap);
			_txtScansel_24.Text = Convert.ToString(_with12.scano_tilt);
			_txtScansel_25.Text = Convert.ToString(_with12.auto_print);
			_txtScansel_26.Text = Convert.ToString(_with12.rawdata_save);
			_txtScansel_27.Text = Convert.ToString(_with12.pro_mode);
			_txtScansel_28.Text = _with12.pro_code.GetString();
			_txtScansel_29.Text = _with12.pro_name.GetString();
			_txtScansel_30.Text = _with12.sliceplan_dir.GetString();
			_txtScansel_31.Text = _with12.slice_plan.GetString();
			_txtScansel_32.Text = Convert.ToString(_with12.auto_zoomflag);
			_txtScansel_33.Text = _with12.autozoom_dir.GetString();
			_txtScansel_34.Text = _with12.auto_zoom.GetString();
			_txtScansel_35.Text = _with12.comment.GetString();
			_txtScansel_36.Text = Convert.ToString(_with12.area_bk);
			_txtScansel_37.Text = Convert.ToString(_with12.speed_bk);
			_txtScansel_38.Text = Convert.ToString(_with12.c_emergency);
			_txtScansel_39.Text = Convert.ToString(_with12.c_scan);
			_txtScansel_40.Text = Convert.ToString(_with12.cpu_ready);
			_txtScansel_41.Text = Convert.ToString(_with12.buzzer);
			_txtScansel_42.Text = Convert.ToString(_with12.print_req);
			_txtScansel_43.Text = Convert.ToString(_with12.panel_lock);
			_txtScansel_44.Text = Convert.ToString(_with12.c_cw_ccw);
			_txtScansel_45.Text = Convert.ToString(_with12.x_ray_auto);
			_txtScansel_46.Text = Convert.ToString(_with12.c_x_on);
			_txtScansel_47.Text = Convert.ToString(_with12.c_x_off);
			_txtScansel_48.Text = Convert.ToString(_with12.c_shut_open);
			_txtScansel_49.Text = Convert.ToString(_with12.c_shut_close);
			_txtScansel_50.Text = Convert.ToString(_with12.c_tra_reset);
			_txtScansel_51.Text = Convert.ToString(_with12.c_rot_reset);
			_txtScansel_52.Text = Convert.ToString(_with12.c_mecha_reset);
			_txtScansel_53.Text = Convert.ToString(_with12.c_count_reset);
			_txtScansel_54.Text = Convert.ToString(_with12.err_reset);
			_txtScansel_55.Text = Convert.ToString(_with12.fapc_reset);
			_txtScansel_56.Text = Convert.ToString(_with12.para_req);
			_txtScansel_57.Text = Convert.ToString(_with12.ud_req);
			_txtScansel_58.Text = Convert.ToString(_with12.ud);
			_txtScansel_59.Text = Convert.ToString(_with12.udab_req);
			_txtScansel_60.Text = Convert.ToString(_with12.udab);
			_txtScansel_61.Text = Convert.ToString(_with12.tr_req);
			_txtScansel_62.Text = Convert.ToString(_with12.tr);
			_txtScansel_63.Text = Convert.ToString(_with12.rot_req);
			_txtScansel_64.Text = Convert.ToString(_with12.rot);
			_txtScansel_65.Text = Convert.ToString(_with12.cnt_req);
			_txtScansel_66.Text = Convert.ToString(_with12.cnt);
			_txtScansel_67.Text = Convert.ToString(_with12.c_focus_reset);
			_txtScansel_68.Text = Convert.ToString(_with12.scan_kv);
			_txtScansel_69.Text = Convert.ToString(_with12.scan_ma);
			_txtScansel_70.Text = Convert.ToString(_with12.scan_view);
			_txtScansel_71.Text = Convert.ToString(_with12.mscan_area);
			_txtScansel_72.Text = Convert.ToString(_with12.mscan_width);
			_txtScansel_73.Text = Convert.ToString(_with12.scano_kv);
			_txtScansel_74.Text = Convert.ToString(_with12.scano_ma);
			_txtScansel_75.Text = Convert.ToString(_with12.mscano_area);
			_txtScansel_76.Text = Convert.ToString(_with12.mscano_width);
			_txtScansel_77.Text = Convert.ToString(_with12.scan_integ_number);
			_txtScansel_78.Text = Convert.ToString(_with12.mscan_bias);
			_txtScansel_79.Text = Convert.ToString(_with12.mscan_slope);
			_txtScansel_80.Text = Convert.ToString(_with12.scano_integ_number);
			_txtScansel_81.Text = Convert.ToString(_with12.mscano_bias);
			_txtScansel_82.Text = Convert.ToString(_with12.mscano_slope);
			_txtScansel_83.Text = Convert.ToString(_with12.scan_and_view);
			_txtScansel_84.Text = Convert.ToString(_with12.image_direction);
			_txtScansel_85.Text = Convert.ToString(_with12.ii_correction);
			_txtScansel_86.Text = Convert.ToString(_with12.fid);
			_txtScansel_87.Text = Convert.ToString(_with12.fcd);
			_txtScansel_88.Text = Convert.ToString(_with12.max_slice_wid);
			
            //_txtScansel_89.Text = _with12.max_scan_area.ToString();
            tmpstr = _with12.max_scan_area.ToStrArray();
            _txtScansel_89.Text = tmpstr[0] + "\r\n" +
                                  tmpstr[1] + "\r\n" +
                                  tmpstr[2];

            _txtScansel_90.Text = Convert.ToString(_with12.max_scano_slice_wid);
			_txtScansel_91.Text = Convert.ToString(_with12.max_scano_area);
			_txtScansel_92.Text = Convert.ToString(_with12.min_slice_wid);
			_txtScansel_93.Text = Convert.ToString(_with12.fimage_bit);
			_txtScansel_94.Text = Convert.ToString(_with12.operation_mode);
			_txtScansel_95.Text = Convert.ToString(_with12.image_rotate_angle);
			_txtScansel_96.Text = Convert.ToString(_with12.recon_mask);
			_txtScansel_97.Text = Convert.ToString(_with12.contrast_fitting);
			_txtScansel_98.Text = Convert.ToString(_with12.max_multislice_pitch);
			_txtScansel_99.Text = Convert.ToString(_with12.multislice);
			_txtScansel_100.Text = Convert.ToString(_with12.multislice_pitch);
			_txtScansel_101.Text = Convert.ToString(_with12.max_multislice);
			_txtScansel_102.Text = Convert.ToString(_with12.fluoro_image_save);
			_txtScansel_103.Text = Convert.ToString(_with12.table_rotation);
			_txtScansel_104.Text = Convert.ToString(_with12.multi_tube);
			_txtScansel_105.Text = Convert.ToString(_with12.mc);
			_txtScansel_106.Text = Convert.ToString(_with12.zp);
			_txtScansel_107.Text = Convert.ToString(_with12.iud);
			_txtScansel_108.Text = Convert.ToString(_with12.k);
			_txtScansel_109.Text = Convert.ToString(_with12.delta_z);
			_txtScansel_110.Text = Convert.ToString(_with12.zs);
			_txtScansel_111.Text = Convert.ToString(_with12.ze);
			_txtScansel_112.Text = Convert.ToString(_with12.delta_msw);
			_txtScansel_113.Text = Convert.ToString(_with12.inh);
			_txtScansel_114.Text = Convert.ToString(_with12.auto_centering);
			_txtScansel_115.Text = Convert.ToString(_with12.cone_scan_area);
			_txtScansel_116.Text = Convert.ToString(_with12.cone_scan_width);
			_txtScansel_117.Text = Convert.ToString(_with12.cone_image_mode);
			_txtScansel_118.Text = Convert.ToString(_with12.cone_max_scan_area);
			_txtScansel_119.Text = Convert.ToString(_with12.fluoro_image_disp);
			_txtScansel_120.Text = Convert.ToString(_with12.binning);
			_txtScansel_121.Text = Convert.ToString(_with12.cone_raw_size);
			_txtScansel_122.Text = _with12.cone_sliceplan_dir.GetString();
			_txtScansel_123.Text = _with12.cone_slice_plan.GetString();
			_txtScansel_124.Text = Convert.ToString(_with12.max_cone_view);
			_txtScansel_125.Text = Convert.ToString(_with12.cone_distribute);
			_txtScansel_126.Text = Convert.ToString(_with12.max_cone_slice_width);
			_txtScansel_127.Text = Convert.ToString(_with12.min_cone_slice_width);
			_txtScansel_128.Text = Convert.ToString(_with12.rotate_select);
			_txtScansel_129.Text = Convert.ToString(_with12.round_trip);
			_txtScansel_130.Text = Convert.ToString(_with12.over_scan);
			_txtScansel_131.Text = Convert.ToString(_with12.disp_size);
			_txtScansel_132.Text = Convert.ToString(_with12.mail_send);
			_txtScansel_133.Text = _with12.smtp_server.GetString();
			_txtScansel_134.Text = _with12.transmitting_person.GetString();
			_txtScansel_135.Text = _with12.address.GetString();
			_txtScansel_136.Text = _with12.carbon_copy.GetString();
			_txtScansel_137.Text = Convert.ToString(_with12.discharge_protect);
			_txtScansel_138.Text = Convert.ToString(_with12.artifact_reduction);
			
            var _with13 =  CTSettings.mecainf.Data;

			_txtMecainf_0.Text = Convert.ToString(_with13.emergency);
			_txtMecainf_1.Text = Convert.ToString(_with13.mecha_ready);
			_txtMecainf_2.Text = Convert.ToString(_with13.mecha_busy);
			_txtMecainf_3.Text = Convert.ToString(_with13.cpu_ready);
			_txtMecainf_4.Text = Convert.ToString(_with13.scan);
			_txtMecainf_5.Text = Convert.ToString(_with13.ups_power);
			_txtMecainf_6.Text = Convert.ToString(_with13.x_ready);
			_txtMecainf_7.Text = Convert.ToString(_with13.test_scan);
			_txtMecainf_8.Text = Convert.ToString(_with13.m_error);
			_txtMecainf_9.Text = Convert.ToString(_with13.rot_mode);
			_txtMecainf_10.Text = Convert.ToString(_with13.ud_ready);
			_txtMecainf_11.Text = Convert.ToString(_with13.ud_busy);
			_txtMecainf_12.Text = Convert.ToString(_with13.ud_error);
			_txtMecainf_13.Text = Convert.ToString(_with13.ud_limit);
			_txtMecainf_14.Text = Convert.ToString(_with13.ud_pos);
			_txtMecainf_15.Text = Convert.ToString(_with13.udab_pos);
			_txtMecainf_16.Text = Convert.ToString(_with13.tr_reset);
			_txtMecainf_17.Text = Convert.ToString(_with13.tr_busy);
			_txtMecainf_18.Text = Convert.ToString(_with13.tr_error);
			_txtMecainf_19.Text = Convert.ToString(_with13.area_size);
			_txtMecainf_20.Text = Convert.ToString(_with13.tr_resets);
			_txtMecainf_21.Text = Convert.ToString(_with13.tr_resetm);
			_txtMecainf_22.Text = Convert.ToString(_with13.tr_resetl);
			_txtMecainf_23.Text = Convert.ToString(_with13.tr_limit);
			_txtMecainf_24.Text = Convert.ToString(_with13.tr_pos);
			_txtMecainf_25.Text = Convert.ToString(_with13.rot_ready);
			_txtMecainf_26.Text = Convert.ToString(_with13.rot_busy);
			_txtMecainf_27.Text = Convert.ToString(_with13.rot_error);
			_txtMecainf_28.Text = Convert.ToString(_with13.rot_reset);
			_txtMecainf_29.Text = Convert.ToString(_with13.rot_180);
			_txtMecainf_30.Text = Convert.ToString(_with13.rot_pos);
			_txtMecainf_31.Text = Convert.ToString(_with13.cnt_ready);
			_txtMecainf_32.Text = Convert.ToString(_with13.cnt_busy);
			_txtMecainf_33.Text = Convert.ToString(_with13.cnt_abnormal);
			_txtMecainf_34.Text = Convert.ToString(_with13.cnt_error);
			_txtMecainf_35.Text = Convert.ToString(_with13.cnt_limit);
			_txtMecainf_36.Text = Convert.ToString(_with13.cnt_pos);
			_txtMecainf_37.Text = Convert.ToString(_with13.filter_ready);
			_txtMecainf_38.Text = Convert.ToString(_with13.filter_busy);
			_txtMecainf_39.Text = Convert.ToString(_with13.filter_error);
			_txtMecainf_40.Text = Convert.ToString(_with13.filter_energy);
			_txtMecainf_41.Text = Convert.ToString(_with13.filter_limit);
			_txtMecainf_42.Text = Convert.ToString(_with13.shutter);
			_txtMecainf_43.Text = Convert.ToString(_with13.colimeter_ready);
			_txtMecainf_44.Text = Convert.ToString(_with13.colimeter_busy);
			_txtMecainf_45.Text = Convert.ToString(_with13.colimeter_error);
			_txtMecainf_46.Text = Convert.ToString(_with13.colimeter_size);
			_txtMecainf_47.Text = Convert.ToString(_with13.colimeter_limit);
			_txtMecainf_48.Text = Convert.ToString(_with13.tube_ready);
			_txtMecainf_49.Text = Convert.ToString(_with13.tube_busy);
			_txtMecainf_50.Text = Convert.ToString(_with13.tube_error);
			_txtMecainf_51.Text = Convert.ToString(_with13.tube_energy);
			_txtMecainf_52.Text = Convert.ToString(_with13.tube_limit);
			_txtMecainf_53.Text = Convert.ToString(_with13.shift_ready);
			_txtMecainf_54.Text = Convert.ToString(_with13.shift_busy);
			_txtMecainf_55.Text = Convert.ToString(_with13.shift_error);
			_txtMecainf_56.Text = Convert.ToString(_with13.shift_size);
			_txtMecainf_57.Text = Convert.ToString(_with13.shift_limit);
			_txtMecainf_58.Text = Convert.ToString(_with13.xray_ready);
			_txtMecainf_59.Text = Convert.ToString(_with13.xray_on);
			_txtMecainf_60.Text = Convert.ToString(_with13.xray_avl);
			_txtMecainf_61.Text = Convert.ToString(_with13.xray_agng);
			_txtMecainf_62.Text = Convert.ToString(_with13.warmup);
			_txtMecainf_63.Text = Convert.ToString(_with13.warmup_days);
			_txtMecainf_64.Text = Convert.ToString(_with13.xray_ovset);
			_txtMecainf_65.Text = Convert.ToString(_with13.xray_slferr);
			_txtMecainf_66.Text = Convert.ToString(_with13.xray_tmp);
			_txtMecainf_67.Text = Convert.ToString(_with13.xray_error);
			_txtMecainf_68.Text = Convert.ToString(_with13.interlock_ready);
			_txtMecainf_69.Text = Convert.ToString(_with13.interlock_open1);
			_txtMecainf_70.Text = Convert.ToString(_with13.interlock_open2);
			_txtMecainf_71.Text = Convert.ToString(_with13.interlock_das);
			_txtMecainf_72.Text = Convert.ToString(_with13.interlock_dwc);
			_txtMecainf_73.Text = Convert.ToString(_with13.power_err);
			_txtMecainf_74.Text = Convert.ToString(_with13.therm_err);
			_txtMecainf_75.Text = Convert.ToString(_with13.ipr);
			_txtMecainf_76.Text = Convert.ToString(_with13.duc);
			_txtMecainf_77.Text = Convert.ToString(_with13.pdo);
			_txtMecainf_78.Text = Convert.ToString(_with13.fapc_ready);
			_txtMecainf_79.Text = Convert.ToString(_with13.fapc_error);
			_txtMecainf_80.Text = Convert.ToString(_with13.slow_position);
			_txtMecainf_81.Text = Convert.ToString(_with13.data_error);
			_txtMecainf_82.Text = Convert.ToString(_with13.x_ray_auto);
			_txtMecainf_83.Text = Convert.ToString(_with13.ram_data_error);
			_txtMecainf_84.Text = Convert.ToString(_with13.line_mode);
			_txtMecainf_85.Text = Convert.ToString(_with13.op_panel);
			_txtMecainf_86.Text = Convert.ToString(_with13.focus_x_ready);
			_txtMecainf_87.Text = Convert.ToString(_with13.focus_x_busy);
			_txtMecainf_88.Text = Convert.ToString(_with13.focus_x_error);
			_txtMecainf_89.Text = Convert.ToString(_with13.focus_x_energy);
			_txtMecainf_90.Text = Convert.ToString(_with13.focus_x_limit);
			_txtMecainf_91.Text = Convert.ToString(_with13.focus_y_ready);
			_txtMecainf_92.Text = Convert.ToString(_with13.focus_y_busy);
			_txtMecainf_93.Text = Convert.ToString(_with13.focus_y_error);
			_txtMecainf_94.Text = Convert.ToString(_with13.focus_y_energy);
			_txtMecainf_95.Text = Convert.ToString(_with13.focus_y_limit);
			_txtMecainf_96.Text = Convert.ToString(_with13.focus_z_ready);
			_txtMecainf_97.Text = Convert.ToString(_with13.focus_z_busy);
			_txtMecainf_98.Text = Convert.ToString(_with13.focus_z_error);
			_txtMecainf_99.Text = Convert.ToString(_with13.focus_z_energy);
			_txtMecainf_100.Text = Convert.ToString(_with13.focus_z_limit);
			_txtMecainf_101.Text = Convert.ToString(_with13.filter);
			_txtMecainf_102.Text = Convert.ToString(_with13.rot_speed_const);
			_txtMecainf_103.Text = Convert.ToString(_with13.phm_ready);
			_txtMecainf_104.Text = Convert.ToString(_with13.phm_busy);
			_txtMecainf_105.Text = Convert.ToString(_with13.phm_error);
			_txtMecainf_106.Text = Convert.ToString(_with13.phm_limit);
			_txtMecainf_107.Text = Convert.ToString(_with13.phm_onoff);
			_txtMecainf_108.Text = Convert.ToString(_with13.vertical_cor);
			_txtMecainf_109.Text = Convert.ToString(_with13.normal_rc_cor);
			_txtMecainf_110.Text = Convert.ToString(_with13.cone_rc_cor);
			_txtMecainf_111.Text = Convert.ToString(_with13.offset_cor);
			_txtMecainf_112.Text = Convert.ToString(_with13.gain_cor);
			_txtMecainf_113.Text = Convert.ToString(_with13.distance0_cor);
			_txtMecainf_114.Text = Convert.ToString(_with13.distance1_cor);
			_txtMecainf_115.Text = Convert.ToString(_with13.scanpos_cor);
			_txtMecainf_116.Text = Convert.ToString(_with13.distance_cor_inh);
			_txtMecainf_117.Text = Convert.ToString(_with13.scanpos_cor_inh);
			_txtMecainf_118.Text = Convert.ToString(_with13.ver_iifield);
			_txtMecainf_119.Text = Convert.ToString(_with13.ver_mt);
			_txtMecainf_120.Text = Convert.ToString(_with13.rc_kv);
			_txtMecainf_121.Text = Convert.ToString(_with13.rc_udab_pos);
			_txtMecainf_122.Text = Convert.ToString(_with13.rc_iifield);
			_txtMecainf_123.Text = Convert.ToString(_with13.rc_mt);
			_txtMecainf_124.Text = Convert.ToString(_with13.gain_iifield);
			_txtMecainf_125.Text = Convert.ToString(_with13.gain_kv);
			_txtMecainf_126.Text = Convert.ToString(_with13.gain_ma);
			_txtMecainf_127.Text = Convert.ToString(_with13.gain_mt);
			_txtMecainf_128.Text = Convert.ToString(_with13.gain_filter);
			_txtMecainf_129.Text = Convert.ToString(_with13.off_date);
			_txtMecainf_130.Text = Convert.ToString(_with13.dc_iifield);
			_txtMecainf_131.Text = Convert.ToString(_with13.dc_mt);
			_txtMecainf_132.Text = Convert.ToString(_with13.sp_iifield);
			_txtMecainf_133.Text = Convert.ToString(_with13.sp_mt);
			_txtMecainf_134.Text = Convert.ToString(_with13.xstg_ready);
			_txtMecainf_135.Text = Convert.ToString(_with13.xstg_busy);
			_txtMecainf_136.Text = Convert.ToString(_with13.xstg_error);
			_txtMecainf_137.Text = Convert.ToString(_with13.xstg_limit);
			_txtMecainf_138.Text = Convert.ToString(_with13.xstg_pos);
			_txtMecainf_139.Text = Convert.ToString(_with13.ystg_ready);
			_txtMecainf_140.Text = Convert.ToString(_with13.ystg_busy);
			_txtMecainf_141.Text = Convert.ToString(_with13.ystg_error);
			_txtMecainf_142.Text = Convert.ToString(_with13.ystg_limit);
			_txtMecainf_143.Text = Convert.ToString(_with13.ystg_pos);
			_txtMecainf_144.Text = Convert.ToString(_with13.table_auto_move);
			_txtMecainf_145.Text = Convert.ToString(_with13.auto_move_xf);
			_txtMecainf_146.Text = Convert.ToString(_with13.auto_move_yf);
			_txtMecainf_147.Text = Convert.ToString(_with13.auto_move_xo);
			_txtMecainf_148.Text = Convert.ToString(_with13.auto_move_yo);
			_txtMecainf_149.Text = Convert.ToString(_with13.iifield);
			_txtMecainf_150.Text = Convert.ToString(_with13.rc_bin);
			_txtMecainf_151.Text = Convert.ToString(_with13.ver_bin);
			_txtMecainf_152.Text = Convert.ToString(_with13.sp_bin);
			_txtMecainf_153.Text = Convert.ToString(_with13.gain_bin);
			_txtMecainf_154.Text = Convert.ToString(_with13.off_bin);
			_txtMecainf_155.Text = Convert.ToString(_with13.dc_bin);
			_txtMecainf_156.Text = Convert.ToString(_with13.dc_rs);
			_txtMecainf_157.Text = Convert.ToString(_with13.rc_rs);
			
            var _with14 = CTSettings.workshopinf.Data;

			_txtWorkshopinf_0.Text = _with14.workshop.GetString();
			
            var _with15 = CTSettings.zoomtbl.Data;

			_txtZoomtbl_1.Text = _with15.commnet.GetString();
			
            var _with16 = CTSettings.reconinf.Data;

			_txtReconinf_0.Text = Convert.ToString(_with16.table_abpos);
			_txtReconinf_1.Text = Convert.ToString(_with16.table_repos);
			_txtReconinf_2.Text = _with16.raw_dir.GetString();
			_txtReconinf_3.Text = _with16.raw_name.GetString();
			_txtReconinf_4.Text = Convert.ToString(_with16.scan_mode);
			_txtReconinf_5.Text = Convert.ToString(_with16.zoomflag);
			_txtReconinf_6.Text = _with16.zooming_dir.GetString();
			_txtReconinf_7.Text = _with16.zooming.GetString();
			_txtReconinf_8.Text = Convert.ToString(_with16.x);
			_txtReconinf_9.Text = Convert.ToString(_with16.y);
			_txtReconinf_10.Text = Convert.ToString(_with16.size);
			_txtReconinf_11.Text = _with16.speed_mode.GetString();
			_txtReconinf_12.Text = _with16.fcno.GetString();
			_txtReconinf_13.Text = _with16.area.GetString();
			_txtReconinf_14.Text = _with16.energy.GetString();
			_txtReconinf_15.Text = _with16.slice_wid.GetString();
			_txtReconinf_16.Text = _with16.det_ap.GetString();
			_txtReconinf_17.Text = _with16.matrix_size.GetString();
			_txtReconinf_18.Text = Convert.ToString(_with16.auto_print);
			_txtReconinf_19.Text = Convert.ToString(_with16.bhcflag);
			_txtReconinf_20.Text = _with16.bhc_dir.GetString();
			_txtReconinf_21.Text = _with16.bhc_name.GetString();
			_txtReconinf_22.Text = Convert.ToString(_with16.zooming_num);
			_txtReconinf_23.Text = Convert.ToString(_with16.mbhc_airLogValue);
			
            var _with17 = CTSettings.sp2inf.Data;

			_txtSp2inf_0.Text = Convert.ToString(_with17.selnum);
			_txtSp2inf_1.Text = Convert.ToString(_with17.pd_start);
			_txtSp2inf_2.Text = Convert.ToString(_with17.rc_start);

            var _with18 = CTSettings.pdplan.Data;

            //_txtPdplan_0.Text = _with18.area.ToString();
            _txtPdplan_0.Text = _with18.area.GetString();
            //_txtPdplan_1.Text = _with18.zx.ToString();
            _txtPdplan_1.Text = _with18.zx.GetString();
            //_txtPdplan_2.Text = _with18.zy.ToString();
            _txtPdplan_2.Text = _with18.zy.GetString();
            //_txtPdplan_3.Text = _with18.size.ToString();
            _txtPdplan_3.Text = _with18.size.GetString();
            //_txtPdplan_4.Text = _with18.pd_dir.ToString();
            _txtPdplan_4.Text = _with18.pd_dir.GetString();
            //_txtPdplan_5.Text = _with18.pd_table.ToString();
            _txtPdplan_5.Text = _with18.pd_table.GetString();
            //_txtPdplan_6.Text = _with18.xs.ToString();
            _txtPdplan_6.Text = _with18.xs[0].ToString() + "\r\n" +
                     _with18.xs[1].ToString() + "\r\n" +
                     _with18.xs[2].ToString() + "\r\n" +
                     _with18.xs[3].ToString() + "\r\n" +
                     _with18.xs[4].ToString() + "\r\n" +
                     _with18.xs[5].ToString() + "\r\n" +
                     _with18.xs[6].ToString() + "\r\n" +
                     _with18.xs[7].ToString() + "\r\n" +
                     _with18.xs[8].ToString() + "\r\n" +
                     _with18.xs[9].ToString() + "\r\n" +
                     _with18.xs[10].ToString() + "\r\n" +
                     _with18.xs[11].ToString() + "\r\n" +
                     _with18.xs[12].ToString() + "\r\n" +
                     _with18.xs[13].ToString() + "\r\n" +
                     _with18.xs[14].ToString() + "\r\n" +
                     _with18.xs[15].ToString() + "\r\n" +
                     _with18.xs[16].ToString() + "\r\n" +
                     _with18.xs[17].ToString() + "\r\n" +
                     _with18.xs[18].ToString() + "\r\n" +
                     _with18.xs[19].ToString();         
            
            //_txtPdplan_7.Text = _with18.ys.ToString();
            _txtPdplan_7.Text = _with18.ys[0].ToString() + "\r\n" +
                    _with18.ys[1].ToString() + "\r\n" +
                    _with18.ys[2].ToString() + "\r\n" +
                    _with18.ys[3].ToString() + "\r\n" +
                    _with18.ys[4].ToString() + "\r\n" +
                    _with18.ys[5].ToString() + "\r\n" +
                    _with18.ys[6].ToString() + "\r\n" +
                    _with18.ys[7].ToString() + "\r\n" +
                    _with18.ys[8].ToString() + "\r\n" +
                    _with18.ys[9].ToString() + "\r\n" +
                    _with18.ys[10].ToString() + "\r\n" +
                    _with18.ys[11].ToString() + "\r\n" +
                    _with18.ys[12].ToString() + "\r\n" +
                    _with18.ys[13].ToString() + "\r\n" +
                    _with18.ys[14].ToString() + "\r\n" +
                    _with18.ys[15].ToString() + "\r\n" +
                    _with18.ys[16].ToString() + "\r\n" +
                    _with18.ys[17].ToString() + "\r\n" +
                    _with18.ys[18].ToString() + "\r\n" +
                    _with18.ys[19].ToString();                    
            
            //_txtPdplan_8.Text = _with18.xe.ToString();
            _txtPdplan_8.Text = _with18.xe[0].ToString() + "\r\n" +
                     _with18.xe[1].ToString() + "\r\n" +
                     _with18.xe[2].ToString() + "\r\n" +
                     _with18.xe[3].ToString() + "\r\n" +
                     _with18.xe[4].ToString() + "\r\n" +
                     _with18.xe[5].ToString() + "\r\n" +
                     _with18.xe[6].ToString() + "\r\n" +
                     _with18.xe[7].ToString() + "\r\n" +
                     _with18.xe[8].ToString() + "\r\n" +
                     _with18.xe[9].ToString() + "\r\n" +
                     _with18.xe[10].ToString() + "\r\n" +
                     _with18.xe[11].ToString() + "\r\n" +
                     _with18.xe[12].ToString() + "\r\n" +
                     _with18.xe[13].ToString() + "\r\n" +
                     _with18.xe[14].ToString() + "\r\n" +
                     _with18.xe[15].ToString() + "\r\n" +
                     _with18.xe[16].ToString() + "\r\n" +
                     _with18.xe[17].ToString() + "\r\n" +
                     _with18.xe[18].ToString() + "\r\n" +
                     _with18.xe[19].ToString();          
            
            //_txtPdplan_9.Text = _with18.ye.ToString();
            _txtPdplan_9.Text = _with18.ye[0].ToString() + "\r\n" +
                    _with18.ye[1].ToString() + "\r\n" +
                    _with18.ye[2].ToString() + "\r\n" +
                    _with18.ye[3].ToString() + "\r\n" +
                    _with18.ye[4].ToString() + "\r\n" +
                    _with18.ye[5].ToString() + "\r\n" +
                    _with18.ye[6].ToString() + "\r\n" +
                    _with18.ye[7].ToString() + "\r\n" +
                    _with18.ye[8].ToString() + "\r\n" +
                    _with18.ye[9].ToString() + "\r\n" +
                    _with18.ye[10].ToString() + "\r\n" +
                    _with18.ye[11].ToString() + "\r\n" +
                    _with18.ye[12].ToString() + "\r\n" +
                    _with18.ye[13].ToString() + "\r\n" +
                    _with18.ye[14].ToString() + "\r\n" +
                    _with18.ye[15].ToString() + "\r\n" +
                    _with18.ye[16].ToString() + "\r\n" +
                    _with18.ye[17].ToString() + "\r\n" +
                    _with18.ye[18].ToString() + "\r\n" +
                    _with18.ye[19].ToString();                    
            
            //_txtPdplan_10.Text = _with18.low.ToString();
            _txtPdplan_10.Text = _with18.low[0].ToString() + "\r\n" +
                    _with18.low[1].ToString() + "\r\n" +
                    _with18.low[2].ToString() + "\r\n" +
                    _with18.low[3].ToString() + "\r\n" +
                    _with18.low[4].ToString() + "\r\n" +
                    _with18.low[5].ToString() + "\r\n" +
                    _with18.low[6].ToString() + "\r\n" +
                    _with18.low[7].ToString() + "\r\n" +
                    _with18.low[8].ToString() + "\r\n" +
                    _with18.low[9].ToString() + "\r\n" +
                    _with18.low[10].ToString() + "\r\n" +
                    _with18.low[11].ToString() + "\r\n" +
                    _with18.low[12].ToString() + "\r\n" +
                    _with18.low[13].ToString() + "\r\n" +
                    _with18.low[14].ToString() + "\r\n" +
                    _with18.low[15].ToString() + "\r\n" +
                    _with18.low[16].ToString() + "\r\n" +
                    _with18.low[17].ToString() + "\r\n" +
                    _with18.low[18].ToString() + "\r\n" +
                    _with18.low[19].ToString();  
               
            //_txtPdplan_11.Text = _with18.high.ToString();
            _txtPdplan_11.Text = _with18.high[0].ToString() + "\r\n" +
                   _with18.high[1].ToString() + "\r\n" +
                   _with18.high[2].ToString() + "\r\n" +
                   _with18.high[3].ToString() + "\r\n" +
                   _with18.high[4].ToString() + "\r\n" +
                   _with18.high[5].ToString() + "\r\n" +
                   _with18.high[6].ToString() + "\r\n" +
                   _with18.high[7].ToString() + "\r\n" +
                   _with18.high[8].ToString() + "\r\n" +
                   _with18.high[9].ToString() + "\r\n" +
                   _with18.high[10].ToString() + "\r\n" +
                   _with18.high[11].ToString() + "\r\n" +
                   _with18.high[12].ToString() + "\r\n" +
                   _with18.high[13].ToString() + "\r\n" +
                   _with18.high[14].ToString() + "\r\n" +
                   _with18.high[15].ToString() + "\r\n" +
                   _with18.high[16].ToString() + "\r\n" +
                   _with18.high[17].ToString() + "\r\n" +
                   _with18.high[18].ToString() + "\r\n" +
                   _with18.high[19].ToString();             

            //_txtPdplan_12.Text = _with18.pt1low.ToString();
            _txtPdplan_12.Text = _with18.pt1low[0].ToString() + "\r\n" +
                  _with18.pt1low[1].ToString() + "\r\n" +
                  _with18.pt1low[2].ToString() + "\r\n" +
                  _with18.pt1low[3].ToString() + "\r\n" +
                  _with18.pt1low[4].ToString() + "\r\n" +
                  _with18.pt1low[5].ToString() + "\r\n" +
                  _with18.pt1low[6].ToString() + "\r\n" +
                  _with18.pt1low[7].ToString() + "\r\n" +
                  _with18.pt1low[8].ToString() + "\r\n" +
                  _with18.pt1low[9].ToString() + "\r\n" +
                  _with18.pt1low[10].ToString() + "\r\n" +
                  _with18.pt1low[11].ToString() + "\r\n" +
                  _with18.pt1low[12].ToString() + "\r\n" +
                  _with18.pt1low[13].ToString() + "\r\n" +
                  _with18.pt1low[14].ToString() + "\r\n" +
                  _with18.pt1low[15].ToString() + "\r\n" +
                  _with18.pt1low[16].ToString() + "\r\n" +
                  _with18.pt1low[17].ToString() + "\r\n" +
                  _with18.pt1low[18].ToString() + "\r\n" +
                  _with18.pt1low[19].ToString();             
            
            //_txtPdplan_13.Text = _with18.pt1high.ToString();
            _txtPdplan_13.Text = _with18.pt1high[0].ToString() + "\r\n" +
                  _with18.pt1high[1].ToString() + "\r\n" +
                  _with18.pt1high[2].ToString() + "\r\n" +
                  _with18.pt1high[3].ToString() + "\r\n" +
                  _with18.pt1high[4].ToString() + "\r\n" +
                  _with18.pt1high[5].ToString() + "\r\n" +
                  _with18.pt1high[6].ToString() + "\r\n" +
                  _with18.pt1high[7].ToString() + "\r\n" +
                  _with18.pt1high[8].ToString() + "\r\n" +
                  _with18.pt1high[9].ToString() + "\r\n" +
                  _with18.pt1high[10].ToString() + "\r\n" +
                  _with18.pt1high[11].ToString() + "\r\n" +
                  _with18.pt1high[12].ToString() + "\r\n" +
                  _with18.pt1high[13].ToString() + "\r\n" +
                  _with18.pt1high[14].ToString() + "\r\n" +
                  _with18.pt1high[15].ToString() + "\r\n" +
                  _with18.pt1high[16].ToString() + "\r\n" +
                  _with18.pt1high[17].ToString() + "\r\n" +
                  _with18.pt1high[18].ToString() + "\r\n" +
                  _with18.pt1high[19].ToString();             
            
            //_txtPdplan_14.Text = _with18.width.ToString();
            _txtPdplan_14.Text = _with18.width[0].ToString() + "\r\n" +
                 _with18.width[1].ToString() + "\r\n" +
                 _with18.width[2].ToString() + "\r\n" +
                 _with18.width[3].ToString() + "\r\n" +
                 _with18.width[4].ToString() + "\r\n" +
                 _with18.width[5].ToString() + "\r\n" +
                 _with18.width[6].ToString() + "\r\n" +
                 _with18.width[7].ToString() + "\r\n" +
                 _with18.width[8].ToString() + "\r\n" +
                 _with18.width[9].ToString() + "\r\n" +
                 _with18.width[10].ToString() + "\r\n" +
                 _with18.width[11].ToString() + "\r\n" +
                 _with18.width[12].ToString() + "\r\n" +
                 _with18.width[13].ToString() + "\r\n" +
                 _with18.width[14].ToString() + "\r\n" +
                 _with18.width[15].ToString() + "\r\n" +
                 _with18.width[16].ToString() + "\r\n" +
                 _with18.width[17].ToString() + "\r\n" +
                 _with18.width[18].ToString() + "\r\n" +
                 _with18.width[19].ToString();                  
            
            _txtPdplan_15.Text = Convert.ToString(_with18.slnum);
			
            var _with19 = CTSettings.discharge_protect.Data;

			_txtDischargeProtect_0.Text = Convert.ToString(_with19.xon_init_kv);
			_txtDischargeProtect_1.Text = Convert.ToString(_with19.xon_rise_time);
			_txtDischargeProtect_2.Text = Convert.ToString(_with19.ct_para1);
			_txtDischargeProtect_3.Text = Convert.ToString(_with19.ct_para2);
			_txtDischargeProtect_4.Text = Convert.ToString(_with19.ct_para3);
			_txtDischargeProtect_5.Text = Convert.ToString(_with19.ct_max_time);
			_txtDischargeProtect_6.Text = Convert.ToString(_with19.it_para1);
			_txtDischargeProtect_7.Text = Convert.ToString(_with19.it_para2);
			_txtDischargeProtect_8.Text = Convert.ToString(_with19.it_para3);
			_txtDischargeProtect_9.Text = Convert.ToString(_with19.it_para4);
			_txtDischargeProtect_10.Text = Convert.ToString(_with19.it_para5);
			_txtDischargeProtect_11.Text = Convert.ToString(_with19.it_para6);
			_txtDischargeProtect_12.Text = Convert.ToString(_with19.it_min_time);

		}
        //*******************************************************************************
        //機　　能： 「自動更新」オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void optAuto_CheckedChanged(object sender, EventArgs e)
        {
            //自動表示用のタイマーをスタート
            tmrUpdate.Enabled = true;

            //更新ボタンを使用不可にする
            cmdUpdate.Enabled = false;
        }
        //*******************************************************************************
        //機　　能： 「手動更新」オプションボタンクリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void optManual_CheckedChanged(object sender, EventArgs e)
        {
            //自動表示用のタイマーを止める
            tmrUpdate.Enabled = false;

            //更新ボタンを使用可にする
            cmdUpdate.Enabled = true;
        }
        //*******************************************************************************
        //機　　能： 自動表示用のタイマーのインターバル変更時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cwneInterval_ValueChanged(object sender, EventArgs e)
        {
            //変更されたインターバルをセットする
            tmrUpdate.Interval = (int)cwneInterval.Value * 1000;
        }
        //*******************************************************************************
        //機　　能： コモンの自動表示用のタイマー処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void tmrUpdate_Tick(object sender, EventArgs e)
        {

            //更新ボタンをクリックした時と同じ処理をする
            cmdUpdate_Click_1(cmdUpdate, new System.EventArgs());

        }
        //*******************************************************************************
        //機　　能： フォームロード時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void frmCommonViewer_Load(object sender, EventArgs e)
        {

            //共有メモリの作成
            hComMap = ComLib.CreateSharedCTCommon();
            if (hComMap == IntPtr.Zero)
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("共有メモリ作成失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);
                //return 1;
                return;
            }
            //共有メモリのセット
            if (ComLib.SetSharedCTCommon() != 0)
            {
                //コメントにしておく  2014/11/19hata
                //MessageBox.Show("共有メモリセット失敗",
                //                    Application.ProductName,
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Error);

                //return 2;
                return;
            }
            //CTSettings.InitializeSharedCTCommon();
            //初期状態は「手動更新」
            optManual.Checked = true;
        }

        //*******************************************************************************
        //機　　能： 「Xrayinf取得」クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdGetXrayinf_Click(object sender, EventArgs e)
        {
            modXrayinfo.XrayInfoStruct theInfo = default(modXrayinfo.XrayInfoStruct);

            modXrayinfo.ReadXrayInfo(ref theInfo);

            _txtXrayinf_0.Text = theInfo.WarmupOk.ToString();
            _txtXrayinf_1.Text = theInfo.WarmupRunning.ToString();
            _txtXrayinf_2.Text = theInfo.xray_filament.ToString();
            _txtXrayinf_3.Text = theInfo.xray_available.ToString();
            _txtXrayinf_4.Text = theInfo.xray_on.ToString();
            _txtXrayinf_5.Text = theInfo.targetCurrent.ToString();
            _txtXrayinf_6.Text = theInfo.fbCurrent.ToString();

        }
        //*******************************************************************************
        //機　　能： 「Xrayinf書き込み」クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdPutXrayinf_Click(object sender, EventArgs e)
        {
            modXrayinfo.XrayInfoStruct theInfo = default(modXrayinfo.XrayInfoStruct);

            theInfo.WarmupOk = Convert.ToInt32(_txtXrayinf_0.Text);
            theInfo.WarmupRunning = Convert.ToInt32(_txtXrayinf_1.Text);
            theInfo.xray_filament = Convert.ToInt32(_txtXrayinf_2.Text);
            theInfo.xray_available = Convert.ToInt32(_txtXrayinf_3.Text);
            theInfo.xray_on = Convert.ToInt32(_txtXrayinf_4.Text);
            theInfo.fbCurrent = Convert.ToSingle(_txtXrayinf_5.Text);
            theInfo.targetCurrent = Convert.ToSingle(_txtXrayinf_6.Text);

            modXrayinfo.WriteXrayInfo(theInfo);
        }
        //*******************************************************************************
        //機　　能： 「Scanstop取得」クリック時処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        private void cmdReadScanstop_Click_1(object sender, EventArgs e)
        {
            scanstop.ScanStopStruct scanstopInfo = default(scanstop.ScanStopStruct);

            scanstop.ReadScanStop(ref scanstopInfo);

            _txtScanstop_0.Text = Convert.ToString(scanstopInfo.scan_stop);
            _txtScanstop_1.Text = Convert.ToString(scanstopInfo.emergency);
            _txtScanstop_2.Text = Convert.ToString(scanstopInfo.ups_power100);
            _txtScanstop_3.Text = Convert.ToString(scanstopInfo.ups_power200);

        }

    }
}
