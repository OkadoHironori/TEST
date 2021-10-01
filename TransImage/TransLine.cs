using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TransImage
{
    /// <summary>
    /// 線タイプ
    /// </summary>
    public enum LineConstants
    {
        ScanLine,       // スキャンライン
        UpperLine,      // コーンビーム時の上端ライン
        LowerLine,      // コーンビーム時の下端ライン
        CenterLine,     // 中心線(縦)
        CenterLineH,    // 中心線(横)
        ProfilePosV,     // ラインプロファイル(垂直位置) //Rev25.00 追加 by長野 2016/08/08
        ProfilePosH,     // ラインプロファイル(水平位置) //Rev25.00 追加 by長野 2016/08/08
        Other
    }

    /// <summary>
    /// 線クラス
    /// </summary>
    public class TransLine
    {
        // タイプ
        private LineConstants lineType = LineConstants.Other;

        //追加2014/10/07hata_v19.51反映
        //ユーザ設定用のペン色
        private Color myUserPenColor = Color.Transparent;
        //ユーザ設定用ペン色有無のフラグ
        private bool myUserPenColorFlg = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type"></param>
        public TransLine(LineConstants type)
        {
            lineType = type;
        }

        /// <summary>
        /// 表示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// タイプ
        /// </summary>
        public LineConstants LineType { get { return lineType; } }

        /// <summary>
        /// 始点
        /// </summary>
        public PointF P1 { get; set; }
        
        /// <summary>
        /// 終点
        /// </summary>
        public PointF P2 { get; set; }
        
        /// <summary>
        /// ユーザカラーの設定
        /// </summary>
        public Color UserCoror
        {
            get
            {
                return myUserPenColor;
            }
            set
            {
                myUserPenColor = value;
            }

        }

        /// <summary>
        /// ユーザカラーを使用するかどうかの設定
        /// </summary>
        public bool UserColorUsed
        {
            get
            {
                return myUserPenColorFlg;
            }
            set
            {
                myUserPenColorFlg = value;
            }
        }

        /// <summary>
        /// 線を描画する
        /// </summary>
        public void Draw(Graphics g)
        {
            if (Visible)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddLine(P1, P2);
                g.DrawPath(GetPen(), path);
                path.Dispose();
            }

            //Draw(g, 0);
        }

        /// <summary>
        /// 線を描画する
        /// </summary>
        public void Draw(Graphics g, int  MirrorOn )
        {
            if (Visible)
            {
                GraphicsPath path = new GraphicsPath();

                if (MirrorOn == 1)
                {
                    path.AddLine(P1.X, P1.Y, P2.X, P2.Y);
                }
                else
                {
                    path.AddLine(P1, P2);
                }

                g.DrawPath(GetPen(), path);
                path.Dispose();
            }
        }


        /// <summary>
        /// ペン選択
        /// </summary>
        /// <returns></returns>
        private Pen GetPen()
        {
            Pen pen = Pens.Red;
             switch (lineType)
            {
                case LineConstants.ScanLine:
                    pen = Pens.Green;
                    break;
                case LineConstants.UpperLine:
                case LineConstants.LowerLine:
                    pen = Pens.Yellow;
                    break;
                case LineConstants.CenterLine:
                case LineConstants.CenterLineH:
                    pen = Pens.Cyan;
                    break;
                case LineConstants.ProfilePosV: //Rev25.00 透視プロファイル 垂直 追加  by長野 2016/08/08
                    pen = Pens.Red;
                    pen.DashStyle = DashStyle.Dash;
                    break;
                case LineConstants.ProfilePosH: //Rev25.00 透視プロファイル 水平 追加 by長野 2016/08/08
                    pen = Pens.Red;
                    pen.DashStyle = DashStyle.Dash;
                    break;
                default:
                    break;
            }
            if (myUserPenColorFlg)
                if (myUserPenColor != Color.Transparent)
                    pen.Color = myUserPenColor;
             
            return pen;
        }
    }
}
