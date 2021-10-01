using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Drawing;
using CT30K.Properties;
using System.IO;
using Microsoft.VisualBasic.FileIO;

//
using CTAPI;
using CT30K.Common;

//-------------------------------------------------------------------
// 間々田　作成Program
// 流用
//
//-------------------------------------------------------------------

namespace CT30K
{
    /// <summary>
    /// 汎用モジュール
    /// </summary>
    static public class modLibrary
    {
        #region 定数データ宣言
        //整数（Integer）の最大値・最小値
        public const short IntMax = short.MaxValue;
        public const short IntMin = short.MinValue;
        #endregion

        #region 最後に発生したｴﾗｰの番号とその内容を表示する
        /// <summary>
        /// 最後に発生したｴﾗｰの番号とその内容を表示する
        /// </summary>
        /// <returns></returns>
        public static DialogResult ErrorDescription(Exception ex)
        {
            return ErrorDescription("", ex);
        }

        ///// <summary>
        ///// 最後に発生したｴﾗｰの番号とその内容を表示する
        ///// </summary>
        ///// <param name="strMsg"></param>
        ///// <returns></returns>
        //public static DialogResult ErrorDescription(Exception ex, string strMsg)
        //{
        //    //メッセージ表示：
        //    //   ｴﾗｰ番号= XXX
        //    //
        //    //   エラー内容
        //    //
        //    //   引数で指定されたメッセージ
        //    //
        //    return MessageBox.Show("エラー発生箇所：" + ex.Source + Constants.vbCrLf + Constants.vbCrLf + ex.Message + Constants.vbCrLf + Constants.vbCrLf + strMsg,
        //                           Application.ProductName,
        //                           MessageBoxButtons.AbortRetryIgnore,
        //                           MessageBoxIcon.Error);
            
        //    //return MessageBox.Show("エラー発生箇所：" + ex.Source + "\r\n" + "\r\n" + ex.Message + "\r\n" + "\r\n" + strMsg,
        //    //                       Application.ProductName,
        //    //                       MessageBoxButtons.AbortRetryIgnore,
        //    //                       MessageBoxIcon.Error);
        //}

        #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
        /*
		Public Function ErrorDescription(Optional ByVal strMsg As String = "")

			'メッセージ表示：
			'   ｴﾗｰ番号= XXX
			'
			'   エラー内容
			'
			'   引数で指定されたメッセージ
			'
			ErrorDescription = MsgBox(LoadResString(IDS_ErrorNum) & Str(Err.Number) & vbCrLf & vbCrLf & _
										Err.Description & vbCrLf & vbCrLf & _
										strMsg, _
										vbCritical + vbAbortRetryIgnore)

		End Function
*/
        #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版
        public static DialogResult ErrorDescription(string strMsg = "", Exception ex = null)		//TODO Errオブジェクト
        {
            //メッセージ表示：
            //   ｴﾗｰ番号= XXX
            //
            //   エラー内容
            //
            //   引数で指定されたメッセージ
            //
            string showMessage = null;

            if (ex == null)
            {
                showMessage = strMsg;
            }
            else
            {
                showMessage = CTResources.LoadResString(StringTable.IDS_ErrorNum) + ex.GetType().Name + "\r\n" + "\r\n" +
                              ex.Message + "\r\n" + "\r\n" +
                              strMsg;
            }

            return MessageBox.Show(showMessage, Application.ProductName, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
        }
        #endregion

        #region ファイル名の禁止文字のチェック(\,/,:,*,?,",<,>,|,スペース)
        /// <summary>
        /// ファイル名の禁止文字のチェック
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>判定結果 true:可  false:不可</returns>
        public static bool FileNameProhibitionCheck(string fileName)
        {
            // ファイル名に使用できない文字の配列
            char[] invalidChars = Path.GetInvalidFileNameChars();

            foreach (var ch in invalidChars)
            {
                if (fileName.Contains(ch)) return false;
            }

            // 追加の禁止文字
            if (fileName.Contains(' ')) return false;   // スペース
            if (fileName.Contains('.')) return false;   // ピリオド     v16.20/v17.00追加 byやまおか 2010/03/02

            return true;
        }
        #endregion

        #region ディレクトリ名の禁止文字のチェック(.,/,*,?,",<,>,|)
        /// <summary>
        /// ディレクトリ名の禁止文字のチェック
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool DirNameProhibitionCheck(string folderName)
        {
            // ファイル名に使用できない文字の配列
            char[] invalidChars = Path.GetInvalidFileNameChars();

            foreach (var ch in invalidChars)
            {
                // 除外する文字
                if (ch.Equals(':')) continue;                   // :
                if (ch.Equals('\\')) continue;                  // \

                if (folderName.Contains(ch)) return false;
            }

            // 追加の禁止文字
            if (folderName.Contains('.')) return false;   // ピリオド     

            return true;
        }

        #endregion
        
        #region 最大値を返す
        /// <summary>
        /// 最大値を返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T MaxVal<T>(params T[] values) where T : IComparable
        {
            T max = values[0];
            for (int i = 1; i < values.Length; ++i)
            {
                if (values[i].CompareTo(max) > 0)
                    max = values[i];
            }
            return max;
        }
        #endregion

        #region 最小値を返す
        /// <summary>
        /// 最小値を返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T MinVal<T>(params T[] values) where T : IComparable
        {
            T min = values[0];
            for (int i = 1; i < values.Length; ++i)
            {
                if (values[i].CompareTo(min) < 0)
                    min = values[i];
            }
            return min;
        }

        #endregion

        #region 文字列を指定された区切り文字で区切った場合の最初の文字列を返す
        /// <summary>
        /// 文字列をコンマで区切った場合の最初の文字列を返す
        /// </summary>
        /// <param name="TargetString"></param>
        /// <returns></returns>
        public static string GetFirstItem(string TargetString)
        {
            return GetFirstItem(TargetString, ",");
        }

        /// <summary>
        /// 文字列を指定された区切り文字で区切った場合の最初の文字列を返す
        /// </summary>
        /// <param name="TargetString"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string GetFirstItem(string TargetString, string delimiter)
        {
            string result = null;
            
            int pos = TargetString.IndexOf(delimiter);
            if (pos < 0) 
            {
                result = TargetString;
            } 
            else
            {
                //result = TargetString.Substring(0, pos - 1);
                //Rev23.20 修正 by長野 2015/12/23
                result = TargetString.Substring(0, pos);
            }

            return result;
        }
        #endregion

        #region フォームが存在するか調べる
        /// <summary>
        /// フォームが存在するか調べる
        /// </summary>
        /// <param name="theForm"></param>
        /// <returns></returns>
        public static bool IsExistForm(Form target)
        {
            // 見つかったら直ちに抜ける
            foreach (var form in Application.OpenForms)
            {
                if (form.Equals(target)) return true;
            }

            return false;
        }

        /// <summary>
        /// フォーム名からフォームが存在するか調べる
        ///　(Instanceを指定するLoadしてしまうため名前で検索する)
        /// </summary>
        /// <param name="theForm"></param>
        /// <returns></returns>
        public static bool IsExistForm(string target)
        {
            // 見つかったら直ちに抜ける
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == target)
                    return true;
            }

            return false;
        }

        #endregion

        #region 値が特定の範囲内か調べる
        /// <summary>
        /// 値が特定の範囲内か調べる
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange<T>(T target, T min, T max) where T : IComparable
        {
            if (target.CompareTo(min) < 0)
                return false;
            else if (target.CompareTo(max) > 0)
                return false;
            else
                return true;
        }
        #endregion

        #region 値を範囲内に収まるように補正します
        /// <summary>
        /// 値を範囲内にする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T CorrectInRange<T>(T target, T min, T max) where T : IComparable
        {
            if (target.CompareTo(min) < 0)
                return min;
            else if (target.CompareTo(max) > 0)
                return max;
            else
                return target;
        }
        #endregion

        #region アークサインを求める
        public static double ArcSin(double theValue)
        {
            return Math.Asin(theValue);
        }
        #endregion

        #region 配列化したラジオボタンのうち，選択したボタンのインデクス値を求める
        /// <summary>
        /// 配列化したラジオボタンのうち，選択したボタンのインデクス値を求める
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static int GetOption(RadioButton[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null && buttons[i].Checked)
                {
                    return i;
                }
            }

            // 見つからなかった場合
            return -1;
        }
        #endregion

        #region 配列化したラジオボタンのうち，指定したインデクス値に相当するボタンを選択する
        /// <summary>
        /// 配列化したラジオボタンのうち，指定したインデクス値に相当するボタンを選択する
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="index"></param>
        /// <param name="MissingValue"></param>
        public static void SetOption(RadioButton[] optArray, int theValue, int? MissingValue = null)
        {
            foreach (RadioButton optUnit in optArray)
            {
                if (Array.IndexOf(optArray, optUnit) == theValue)
                {
                    optUnit.Checked = true;
                    return;
                }
            }

            if (MissingValue != null)
            {
                foreach (RadioButton optUnit in optArray)
                {
                    if (Array.IndexOf(optArray, optUnit) == MissingValue)
                    {
                        optUnit.Checked = true;
                        return;
                    }
                }
            }
        }
        #endregion

        #region 配列化したボタンのうち，指定したインデクス値に相当するボタンを選択する
        /// <summary>
        /// 配列化したボタンのうち，指定したインデクス値に相当するボタンを選択する
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="theValue"></param>
        public static void SetCmdButton(Button[] buttons, int theValue)
        {
            SetCmdButton(buttons, theValue, Color.Lime, SystemColors.Control, false);
        }

        /// <summary>
        /// 配列化したボタンのうち，指定したインデクス値に相当するボタンを選択する
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="theValue"></param>
        /// <param name="ControlEnabled"></param>
        public static void SetCmdButton(Button[] buttons, int theValue, bool ControlEnabled)
        {
            SetCmdButton(buttons, theValue, Color.Lime, SystemColors.Control, ControlEnabled);
        }

        /// <summary>
        /// 配列化したボタンのうち，指定したインデクス値に相当するボタンを選択する
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="theValue"></param>
        /// <param name="OnColor"></param>
        /// <param name="OffColor"></param>
        public static void SetCmdButton(Button[] buttons, int theValue, Color OnColor, Color OffColor)
        {
            SetCmdButton(buttons, theValue, OnColor, OffColor, false);
        }

        /// <summary>
        /// 配列化したボタンのうち，指定したインデクス値に相当するボタンを選択する
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="theValue"></param>
        /// <param name="OnColor"></param>
        /// <param name="OffColor"></param>
        /// <param name="ControlEnabled"></param>
        public static void SetCmdButton(Button[] buttons, int theValue, Color OnColor, Color OffColor, bool ControlEnabled)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] == null) continue;

                buttons[i].BackColor = (i == theValue) ? OnColor : OffColor;

                // Enabledプロパティを制御する場合
                if (ControlEnabled)
                    buttons[i].Enabled = (i != theValue) && buttons[i].Parent.Enabled;
            }
        }
        #endregion

        #region 選択しているボタンのインデクス値を求める
        /// <summary>
        /// 選択しているコマンドボタンのインデクス値を求める
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static int GetCmdButton(Button[] buttons)
        {
            return GetCmdButton(buttons, Color.Lime);
        }

        /// <summary>
        /// 選択しているコマンドボタンのインデクス値を求める
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="OnColor"></param>
        /// <returns></returns>
        public static int GetCmdButton(Button[] buttons, Color OnColor)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null && buttons[i].BackColor == OnColor)
                {
                    return i;
                }
            }

            // 見つからなかった場合
            return -1;
        }        
        #endregion

        #region ラジオボタンの選択調整
        /// <summary>
        /// ラジオボタンの選択調整
        /// 選択しているボタンが使用不可の場合、別のボタンを選択するよう調整する
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns>true: 調整した，false: 調整されなかった</returns>
        public static bool CorrectOption(RadioButton[] buttons)
        {
            foreach (var button in buttons)
            {
                if (button.Checked & button.Enabled)
                    return false;
            }

            foreach (var button in buttons)
            {
                if (button.Enabled)
                {
                    button.Checked = true;
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region ラジオボタンの位置調整
        /// <summary>
        /// ラジオボタンの位置調整
        /// </summary>
        /// <param name="buttons"></param>
        public static void RePosOption(RadioButton[] buttons)
        {
            int count = 0;
            int top = 0;

            // 使用可能なボタンをカウントアップ
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    if (button.Enabled)
                        count++;
                }
            }

            foreach (var button in buttons)
            {
                if (button != null)
                {

                    if (button.Enabled)
                    {
                        //2014/11/13hata キャストの修正
                        //top += button.Parent.Height / (count + 1);
                        top += Convert.ToInt32(button.Parent.Height / (float)(count + 1));
                        button.Top = top;
                    }
                    else
                    {
                        // 使用不可のボタンは見えなくする
                        button.Visible = false;
                    }
                }
            }
        }
        #endregion

        #region ラジオボタンの位置調整2 //Rev20.00 追加 by長野 2015/01/16
        /// <summary>
        /// ラジオボタンの位置調整2 ギリギリで親のフレームをはみ出す場合に使用してください。（全体的に上に動きます。)
        /// </summary>
        /// <param name="buttons"></param>
        public static void RePosOption2(RadioButton[] buttons, int AdjustPoint)
        {
            int count = 0;
            int top = 0;

            // 使用可能なボタンをカウントアップ
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    if (button.Enabled)
                        count++;
                }
            }

            foreach (var button in buttons)
            {
                if (button != null)
                {

                    if (button.Enabled)
                    {
                        //2014/11/13hata キャストの修正
                        //top += button.Parent.Height / (count + 1);
                        top += Convert.ToInt32(button.Parent.Height / (float)(count + 1));
                        button.Top = top - AdjustPoint;
                    }
                    else
                    {
                        // 使用不可のボタンは見えなくする
                        button.Visible = false;
                    }
                }
            }
        }

        #endregion

        #region ラジオボタンの表示有無設定
        //'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここから
        /// <summary>
        /// ラジオボタンの位置調整
        /// </summary>
        /// <param name="buttons"></param>
        public static void RePosVisibleOption(RadioButton[] buttons)
        {
            int count = 0;
            int top = 0;

            // 使用可能なボタンをカウントアップ
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    if (button.Visible)
                        count++;
                }
            }

            foreach (var button in buttons)
            {
                if (button != null)
                {

                    if (button.Enabled)
                    {
                        //2014/11/13hata キャストの修正
                        //top += button.Parent.Height / (count + 1);
                        top += Convert.ToInt32(button.Parent.Height / (float)(count + 1));
                        button.Top = top;
                    }
                    else
                    {
                        // 使用不可のボタンは見えなくする
                        button.Visible = false;
                    }
                }
            }
        }
        //'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで
        #endregion
        #region ラジオボタンの表示有無設定2
        //'v21.00 追加 ギリギリで親のフレームをはみ出す場合に使用してください。（全体的に上に動きます。)
        /// <summary>
        /// ラジオボタンの位置調整2
        /// </summary>
        /// <param name="buttons"></param>
        public static void RePosVisibleOption2(RadioButton[] buttons, int AdjustPoint)
        {
            int count = 0;
            int top = 0;

            // 使用可能なボタンをカウントアップ
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    if (button.Visible)
                        count++;
                }
            }

            foreach (var button in buttons)
            {
                if (button != null)
                {

                    if (button.Visible)
                    {
                        //2014/11/13hata キャストの修正
                        //top += button.Parent.Height / (count + 1);
                        top += Convert.ToInt32(button.Parent.Height / (float)(count + 1));
                        button.Top = top - AdjustPoint;
                    }
                    else
                    {
                        // 使用不可のボタンは見えなくする
                        button.Visible = false;
                    }
                }
            }
        }
        #endregion

        #region 拡張子を付加します
        /// <summary>
        /// 拡張子を付加します
        /// </summary>
        /// <param name="target"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string AddExtension(string target, string extension)
        {
            string result = target;

            if (string.IsNullOrEmpty(target))
            {
            } 
            else if (Strings.LCase(Strings.Right(target, Strings.Len(extension))) != Strings.LCase(extension))
            {
                result = target + extension;
            }

            return result;
        }
        #endregion

        #region 拡張子を除去します
        /// <summary>
        /// 拡張子を除去します
        /// </summary>
        /// <param name="target"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string RemoveExtension(string target, string extension)
        {
            string result = target;

            if (string.IsNullOrEmpty(target))
            {
            } 
            else if (Strings.LCase(Strings.Right(target, Strings.Len(extension))) == Strings.LCase(extension))
            {
                result = Strings.Left(target, Strings.Len(target) - Strings.Len(extension));
            }
            return result;
        }
        #endregion

        #region スワップ
        /// <summary>
        /// スワップ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Swap(ref int a, ref int b)
        {
            int w = a;
            a = b;
            b = w;
        }
        #endregion

        #region 指定されたCSVファイルからキーワードに対応する文字列を取得する
        /// <summary>
        /// 指定されたCSVファイルからキーワードに対応する文字列を取得する
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public static string GetCsvItem(string fileName, string keyWord)
        {
            // 戻り値用変数
            string result = string.Empty;

            try
            {
                using (var parser = new TextFieldParser(fileName, Encoding.Default))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(","); // 区切り文字はコンマ

                    while (!parser.EndOfData)
                    {
                        // 1行読み込み
                        string[] row = parser.ReadFields(); 

                        // 最初の項目がキーワードと同じ？
                        if (row[0] == keyWord) 
                        {
                            result = string.Join(",", row.Skip(1).ToArray());
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //エラーメッセージ表示
                MessageBox.Show(ex.Message,
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            return result;
        }
        #endregion

        #region ヌル以降の文字列を取り除く
        /// <summary>
        /// ヌル以降の文字列を取り除く
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string RemoveNull(string target)
        {
            return target.TrimEnd('\0');
        }
        #endregion

        #region 文字列内のCR・CFを半角スペースに置き換える
        /// <summary>
        /// 文字列内のCR・CFを半角スペースに置き換える
        /// </summary>
        /// <param name="target">処理対象文字列</param>
        /// <returns>処理結果文字列</returns>
        public static string RemoveCRLF(string target)
        {
            return target.Replace(Constants.vbCr, " ").Replace(Constants.vbLf, " ");
        }
        #endregion

        #region 指定されたフレーム内のコントロールの使用可・不可を設定する（使用しないでください）
        /// <summary>
        /// 指定されたフレーム内のコントロールの使用可・不可を設定する
        /// .NETではグループボックスを使用不可にすると内包するコントロールも正しくグレーになるので不要
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="theOnOff"></param>
        /// <param name="exceptContorls"></param>
        public static void SetEnabledInFrame(GroupBox frame, bool theOnOff, params string[] exceptContorls)
        {
            var controls = modLibrary.GetAllControls(frame);

            foreach (Control control in controls)
            {
                //if (!exceptContorls.Contains(Information.TypeName(control)))
                if (!exceptContorls.Contains(control.GetType().Name))
                    control.Enabled = theOnOff;
            }
        }
        #endregion

        #region 指定した値に対応する書式文字列を取得する
        /// <summary>
        /// 指定した値に対応する書式文字列を取得する
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetFormatString<T>(T target) 
        {
            string result = Convert.ToString(target);

            // 数字をすべて 0 にする
            for (short i = 1; i <= 9; i++)
            {
                result = result.Replace(Convert.ToString(i), "0");
            }

            return result;
        }
        #endregion

        #region 指定された文字とマッチする文字列配列のインデクス値を求める（廃止：FixedStringクラスに移動）
        ///// <summary>
        ///// 指定された文字とマッチする文字列配列のインデクス値を求める（廃止：FixedStringクラスに移動）
        ///// </summary>
        ///// <param name="theString"></param>
        ///// <param name="theArray"></param>
        ///// <param name="MissingValue"></param>
        ///// <returns></returns>
        //public static int GetIndexByStr(string theString, string[] theArray, int MissingValue = 0)
        //{
        //    int functionReturnValue = 0;

        //    int i = 0;

        //    //戻り値初期化
        //    functionReturnValue = MissingValue;

        //    for (i = theArray.GetLowerBound(0); i <= theArray.GetUpperBound(0); i++)
        //    {
        //        if (theArray[i].Trim().ToUpperInvariant() == theString.Trim().ToUpperInvariant())
        //        {
        //            functionReturnValue = i;
        //            return functionReturnValue;
        //        }
        //    }
        //    return functionReturnValue;
        //}
        #endregion

        #region 固定長文字列変数に文字列をセットする（使用しないでください）
        /// <summary>
        /// 固定長文字列変数に文字列をセットする
        /// （このメソッドの代わりにFixedString.SetStringを使用してください）
        /// </summary>
        /// <param name="theData"></param>
        /// <param name="theFiled"></param>
        public static void SetField(string theData, ref string theFiled)
        {
            theFiled = Strings.Trim(theData).PadRight(Strings.Len(theFiled), '\0');
        }
        #endregion

        #region ﾄﾞライブの空き容量（Kバイト単位）を取得
        /// <summary>
        /// ﾄﾞライブの空き容量（Kバイト単位）を取得
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        public static long GetDiskFreeKByte(string driveName)
        {
            // 戻り値用変数
            long result = -1;

            try
            {
                //// ドライブ情報
                //if (!string.IsNullOrEmpty(driveName))
                //{
                //    var info = new DriveInfo(driveName);
                    
                //    // 現在ユーザーの空き容量
                //    //2014/11/13hata キャストの修正
                //    //result = info.AvailableFreeSpace / 1024;
                //    //result = Convert.ToInt64(info.AvailableFreeSpace / (double)1024);
                //    result = Convert.ToInt64(info.AvailableFreeSpace / (double)1024);
                //}
                //Rev23.00 test
                // ドライブ情報 //ローカルPC
                Uri pathUni = new Uri(driveName);
                if (pathUni.IsUnc)
                {
                    string lpDirectoryName = driveName + "\\";
                    ulong lpFreeBytesAvailable = 0;
                    ulong lpTotalNumberOfBytes = 0;
                    ulong lpNumberOfFreeBytes = 0;
                    Winapi.GetDiskFreeSpaceEx(lpDirectoryName, ref lpFreeBytesAvailable, ref lpTotalNumberOfBytes, ref lpNumberOfFreeBytes);

                    result = Convert.ToInt64(lpFreeBytesAvailable / (double)1024);
                }
                else
                {
                    var info = new DriveInfo(driveName);
                    result = Convert.ToInt64(info.AvailableFreeSpace / (double)1024);
                }
                    // 現在ユーザーの空き容量
                    //2014/11/13hata キャストの修正
                    //result = info.AvailableFreeSpace / 1024;
                    //result = Convert.ToInt64(info.AvailableFreeSpace / (double)1024);
                    //result = Convert.ToInt64(info.AvailableFreeSpace / (double)1024);
                //}
            }
            catch(Exception ex)
            {
                int error = 0;
                error = 128;
            }

            return result;
        }
        #endregion

        #region 指定された文字列配列をコンマで結合します
        /// <summary>
        /// 指定された文字列配列をコンマで結合します
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetCsvRec(params object[] p)
        {
            return Strings.Join(p, ",");
        }
        #endregion

        #region 指定されたコントロールをセンタリングする
        /// <summary>
        /// 指定されたコントロールをセンタリングする    v15.0追加 by 間々田 2009/01/15
        /// </summary>
        /// <param name="control"></param>
	    public static void CenteringInFrame(Control control)
	    {
            try
            {
                var parent = control.Parent;
                //2014/11/13hata キャストの修正
                //control.Location = new Point(parent.Width / 2 - control.Width / 2, parent.Height / 2 - control.Height / 2);
                control.Location = new Point(Convert.ToInt32(parent.Width / 2F - control.Width / 2F), Convert.ToInt32(parent.Height / 2F - control.Height / 2F));
            }
            catch
            {
            }
        }
        #endregion

        #region NumericUpDownのDecimalPlaces/Increment/Minimum/Maximum/Valueプロパティをコピーする
        /// <summary>
        /// NumericUpDownのDecimalPlaces/Increment/Minimum/Maximum/Valueプロパティをコピーする
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        //public static void CopyNumericUpDown(NumericUpDown source, NumericUpDown destination)
        //{
        //    destination.DecimalPlaces = source.DecimalPlaces;
        //    destination.Increment = source.Increment;
        //    destination.Minimum = source.Minimum;
        //    destination.Maximum = source.Maximum;
        //    destination.Value = source.Value;
        //}
        public static void CopyCWNumEdit(NumericUpDown cwneSource, NumericUpDown cwneDestination)
        {
            #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
            /*
			With cwneDestination
				.DiscreteInterval = cwneSource.DiscreteInterval
				.IncDecValue = cwneSource.IncDecValue
				.FormatString = cwneSource.FormatString
				.SetMinMax cwneSource.Minimum, cwneSource.Maximum
				.Value = cwneSource.Value
			End With
*/
            #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            cwneDestination.Increment = cwneSource.Increment;
            cwneDestination.DecimalPlaces = cwneSource.DecimalPlaces;
            cwneDestination.Maximum = cwneSource.Maximum;
            cwneDestination.Minimum = cwneSource.Minimum;
            cwneDestination.Value = cwneSource.Value;

        }

        #endregion

        #region 指定されたフォームに含まれるすべてのラベルのフォントを"Arial"に設定します
        /// <summary>
        /// 指定されたフォームに含まれるすべてのラベルのフォントを"Arial"に設定します
        /// </summary>
        /// <param name="form"></param>
	    public static void SetLabelFont(Form form)
	    {
            SetLabelFont(form, "Arial");
	    }
        #endregion

        #region 指定されたフォームに含まれるすべてのラベルのフォントを設定します
        /// <summary>
        /// 指定されたフォームに含まれるすべてのラベルのフォントを設定します
        /// </summary>
        /// <param name="form"></param>
        /// <param name="fontName"></param>
	    public static void SetLabelFont(Form form, string fontName)
	    {
            // 指定されたフォームに含まれるすべてのコントロールを取得します
            var controls = GetAllControls(form);

            foreach (Control control in controls)
            {			    
                if (control is Label) 
                {
                    control.Font = new Font(fontName, control.Font.Size);
                }
		    }
        }
        #endregion

        #region 指定されたコントロールに含まれるすべてのコントロールを取得します
        /// <summary>
        /// 指定されたコントロールに含まれるすべてのコントロールを取得します
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static Control[] GetAllControls(Control top)
        {
            ArrayList buf = new ArrayList();
            foreach (Control c in top.Controls)
            {
                buf.Add(c);
                buf.AddRange(GetAllControls(c));
            }
            return (Control[])buf.ToArray(typeof(Control));
        }
        #endregion


        //v19.00 追加 ->(電S2)永井

        #region ヌルを付加する
        //*******************************************************************************
        //機　　能： ヌルを付加する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： TargetString    [I/ ] String    処理対象文字列
        //           theLength       [I/ ] Integer   ヌル付加後の全体の文字長を指定（未指定時は256）
        //戻 り 値：                 [ /O] String    処理結果文字列
        //
        //補　　足： なし
        //
        //履　　歴： v6.00 2005/03/11 (SI3)間々田    新規作成
        //*******************************************************************************
        public static string AddNull(string TargetString, int theLength = 256)
        {
            string theString = null;

            theString = TargetString.Trim();

            return theString + new string('\0', MaxVal(theLength - CTAPI.Winapi.lstrlen(theString), 0));
        }
        #endregion


        #region コモンダイアログ用フィルタに設定する文字列を作成する
        //*******************************************************************************
        //
        //   コモンダイアログ用フィルタに設定する文字列を作成する
        //
        //*******************************************************************************
        public static string MakeCommonDialogFilter(string fileType, string Extension)
        {
            //変更2014/11/05hata_ダイアログFilter内容
            //Filter内容が通常の形と違う"*-BHC.csv"等の場合は（*-BHC.csv）の説明文を付加しない。(*-BHC.csv)(*-BHC.csv)と2つ表示されるため。
            //xxx(*yyy)|*yyy|すべてのファイル(*.*)|*.*
            //return fileType + "(*" + Extension + ")|*" + Extension + "|" + CTResources.LoadResString(10300);
            string ExtensionMsg = "(*" + Extension + ")";
            int pos = Extension.LastIndexOf(".");
            if (pos > 0) ExtensionMsg = "";

            return fileType + ExtensionMsg + "|*" + Extension + "|" + CTResources.LoadResString(10300);
        }
        #endregion

        #region コモンダイアログ（ファイル選択［複数指定有］）にて取得したファイル名のリストからパス名とファイル名に分け、配列に格納して返す
        //********************************************************************************
        //機    能  ：  コモンダイアログ（ファイル選択［複数指定有］）にて取得したファイル名のリストから
        //              パス名とファイル名に分け、配列に格納して返す
        //
        //              変数名           [I/O] 型        内容
        //引    数  ：  fileNameList     [I/ ] String    コモンダイアログから受け取るファイル名のリスト
        //戻 り 値  ：  FileName()       [ /O] String    FileName(0):パス名（末尾に \ がつかないタイプ。ルート直下は \ がつく）
        //                                               FileName(n):フルファイル名 （n>0）
        //補    足  ：  なし
        //
        //履    歴  ：  V4.00  02/10/22  (SI4)間々田     新規作成
        //               5.00  04/08/27  (SI4)間々田     引数追加
        //********************************************************************************
//        public static void GetFileList(string fileNameList, ref string[] FileName, bool multiOn = true)
//        {

//            string[] strCell = null;
//            int YenPos = 0;
//            int i = 0;
//            int fileCount = 0;

//            //コモンダイアログで取得したファイル名のリストを文字列配列に分割
//            //追加 v5.00 04/08/27
//            if (multiOn)
//            {
//                //strCell = Split(fileNameList, " ")
//                strCell = fileNameList.Split('\0');				//変更 2005/03/02 コモンダイアログ呼び出し時にcdlOFNExplorerを追加したため
//            }
//            else
//            {									//追加 v5.00 04/08/27
//                strCell = new string[1];		//追加 v5.00 04/08/27
//            }									//追加 v5.00 04/08/27

//            //取得したファイルの数およびファイル名を格納する配列の宣言
//            fileCount = strCell.GetUpperBound(0) == 0 ? 1 : strCell.GetUpperBound(0);
//            FileName = new string[fileCount + 1];

//            //単数ファイル指定
//            if (fileCount == 1)
//            {
//                YenPos = fileNameList.LastIndexOf("\\");
//                FileName[0] = fileNameList.Substring(0, YenPos);

//                //パスの末尾が : の場合、\ を付加
//                if (FileName[0].EndsWith(":")) FileName[0] = FileName[0] + "\\";

//                FileName[1] = fileNameList;
//            }
//            //複数ファイル指定
//            else
//            {
//                //パス名が格納されているstrCell(0)をそのまま FileName(0) にコピー
//                FileName[0] = strCell[0];

//                //パス名の末尾が \ でない場合、\ を付加
//                if (!strCell[0].EndsWith("\\")) strCell[0] = strCell[0] + "\\";

//                //v6.0削除ここから by 間々田 2005/03/11
//                //For i = 1 To fileCount
//                //    fileName(i) = strCell(0) & strCell(i)
//                //Next
//                //v6.0削除ここまで by 間々田 2005/03/11

//                //v6.0追加ここから by 間々田 2005/03/11

//                #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
//                /*
//                Dim theFileLists    As New Collection
//                Dim theFile         As Variant
        
//                'コレクションにソートしながら格納
//                For i = 1 To fileCount
//                    For Each theFile In theFileLists
//                        If strCell(i) < theFile Then Exit For
//                    Next
//                    If IsEmpty(theFile) Then
//                        theFileLists.Add strCell(i), strCell(i)
//                    Else
//                        theFileLists.Add strCell(i), strCell(i), before:=theFile
//                    End If
//                Next
//*/
//                #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

//                SortedList<string, string> theFileLists = new SortedList<string, string>();

//                //コレクションにソートしながら格納
//                for (i = 1; i <= fileCount; i++)
//                {
//                    theFileLists.Add(strCell[i], strCell[i]);
//                }

//                //配列に格納
//                i = 0;
//                foreach (string theFile in theFileLists.Values)
//                {
//                    i = i + 1;
//                    FileName[i] = strCell[0] + theFile;
//                }
//                //v6.0追加ここまで by 間々田 2005/03/11
//            }

//            //'念のためすべての文字列を大文字にしておく   'v7.0削除 by 間々田 2006/02/10
//            //For i = 0 To fileCount
//            //    fileName(i) = UCase$(fileName(i))
//            //Next
//        }
        public static void GetFileList(string[] fileNameList, ref string[] FileName, bool multiOn = true)
        {

            string[] strCell = null;
            int YenPos = 0;
            int i = 0;
            int fileCount = 0;

            //コモンダイアログで取得したファイル名のリストを文字列配列に分割
            //追加 v5.00 04/08/27
            if (multiOn)
            {
                ////strCell = Split(fileNameList, " ")
                //strCell = fileNameList.Split('\0');				//変更 2005/03/02 コモンダイアログ呼び出し時にcdlOFNExplorerを追加したため

                strCell = fileNameList;				//変更 2005/03/02 コモンダイアログ呼び出し時にcdlOFNExplorerを追加したため
            }
            else
            {									//追加 v5.00 04/08/27
                strCell = new string[1];		//追加 v5.00 04/08/27
            }									//追加 v5.00 04/08/27

            //取得したファイルの数およびファイル名を格納する配列の宣言
            //fileCount = strCell.GetUpperBound(0) == 0 ? 1 : strCell.GetUpperBound(0) + 1;
            fileCount = strCell.Length;
            FileName = new string[fileCount + 1];

            //単数ファイル指定
            if (fileCount == 1)
            {
                YenPos = fileNameList[0].LastIndexOf("\\");
                FileName[0] = fileNameList[0].Substring(0, YenPos);

                //パスの末尾が : の場合、\ を付加
                if (FileName[0].EndsWith(":")) FileName[0] = FileName[0] + "\\";

                FileName[1] = fileNameList[0];
            }
            //複数ファイル指定
            else
            {
                //パス名が格納されているstrCell(0)をそのまま FileName(0) にコピー
                YenPos = fileNameList[0].LastIndexOf("\\");
                FileName[0] = fileNameList[0].Substring(0, YenPos);

                //パスの末尾が : の場合、\ を付加
                if (FileName[0].EndsWith(":")) FileName[0] = FileName[0] + "\\";

                SortedList<string, string> theFileLists = new SortedList<string, string>();

                //コレクションにソートしながら格納
                for (i = 0; i < fileCount; i++)
                {
                    theFileLists.Add(strCell[i], strCell[i]);
                }

                //配列に格納
                i = 0;
                foreach (string theFile in theFileLists.Values)
                {
                    i = i + 1;
                    FileName[i] = theFile;
                }
            }

            //'念のためすべての文字列を大文字にしておく   'v7.0削除 by 間々田 2006/02/10
            //For i = 0 To fileCount
            //    fileName(i) = UCase$(fileName(i))
            //Next
        }
 
        
        #endregion

        #region テキストボックスバイト数チェック（全角文字対応版）
        //*******************************************************************************
        //機　　能： テキストボックスバイト数チェック（全角文字対応版）
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  2004/06/01 (SI4)間々田   新規作成
        //*******************************************************************************
        public static void CheckTextBox(TextBox theTextBox)
        {

            #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
            /*
			Dim ansiStr     As String

			With theTextBox

				'イベントの連鎖防止
				If Not .Enabled Then Exit Sub

				'バイト数チェックのためansiコードに変換する
				ansiStr = StrConv(.Text, vbFromUnicode)

				'バイト数がMaxLengthを超えたら
				If LenB(ansiStr) > .MaxLength Then

					'メッセージボックスの表示：%1文字を越えています。
					'v19.00 マイクロCTにあわせる
					'ShowMessage GetResString(9928, CStr(.MaxLength)), vbCritical
					ShowMessage GetResString(9318, CStr(.MaxLength))

					'強制的にMaxLengthバイトにする。unicodeに戻す
					.Enabled = False
					.Text = StrConv(LeftB(ansiStr, .MaxLength), vbUnicode)
					.Enabled = True

				End If

			End With
*/
            #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //イベントの連鎖防止
            if (!theTextBox.Enabled) return;

            string tbText = theTextBox.Text;

            //バイト数がMaxLengthを超えたら
            if (Encoding.Default.GetByteCount(tbText) > theTextBox.MaxLength)
            {
                //メッセージボックスの表示：%1文字を越えています。
                //v19.00 マイクロCTにあわせる
                //ShowMessage GetResString(9928, CStr(.MaxLength)), vbCritical
                modCT30K.ShowMessage(StringTable.GetResString(9318, theTextBox.MaxLength.ToString()));

                //強制的にMaxLengthバイトにする。
                string cutText = Encoding.Default.GetString(Encoding.Default.GetBytes(tbText), 0, theTextBox.MaxLength);

                if (cutText[cutText.Length - 1] != tbText[cutText.Length - 1])
                {
                    cutText = cutText.Substring(0, cutText.Length - 1);
                }

                theTextBox.Enabled = false;
                theTextBox.Text = cutText;
                theTextBox.Enabled = true;
            }
        }
        #endregion


        #region フルファイル名をディレクトリ名とファイル名に分ける
        //*******************************************************************************
        //機　　能： フルファイル名をディレクトリ名とファイル名に分ける
        //
        //           変数名          [I/O] 型        内容
        //引　　数： FullFileName    [I/ ] String    フルファイル名
        //           DirName         [ /O] String    ディレクトリ名（末尾に\が付いている）
        //           FileName        [ /O] String    ファイル名
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v8.2  2007/05/09  (WEB)間々田  新規作成
        //*******************************************************************************
        public static void SeparateFileName(string FullFileName, ref string DirName, ref string FileName)
        {
            int YenPos = 0;

            YenPos = FullFileName.LastIndexOf("\\");
            DirName = FullFileName.Substring(0, YenPos);
            FileName = FullFileName.Substring(YenPos + 1);
        }
        #endregion

        #region v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        #region ディレクトリ名およびファイル名のチェック関数
        //*******************************************************************************
        //機　　能： ディレクトリ名およびファイル名のチェック関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： DirName         [I/ ] String
        //
        //戻 り 値：                 [ /O] Boolean
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Public Function IsValidFileName(ByVal DirName As String, ByVal FileName As String) As Boolean
        //
        //    Dim hFso As FileSystemObject
        //    Set hFso = New FileSystemObject
        //
        //'v8.0 削除 by 村田
        //    '戻り値初期化
        //'    IsValidFileName = False
        //'
        //'    If DirName = "" Then                                'ディレクトリ名を指定していない
        //'        Exit Function
        //'    ElseIf Dir(DirName, vbDirectory) = "" Then          'ディレクトリが存在しない
        //'        Exit Function
        //'    ElseIf fileName = "" Then                           'ファイル名を指定していない
        //'        Exit Function
        //'    ElseIf Not FileNameProhibitionCheck(fileName) Then  'ファイル名に禁止文字が含まれている
        //'        Exit Function
        //'    End If
        //'
        //'    IsValidFileName = True
        //
        //    '戻り値初期化
        //    IsValidFileName = True
        //
        //    'フォルダの存在確認
        //    If Not hFso.FolderExists(DirName) Then
        //        IsValidFileName = False
        //    'ファイルの存在確認
        //    ElseIf Not hFso.FileExists(DirName & FileName) Then
        //        IsValidFileName = False
        //    ElseIf Not FileNameProhibitionCheck(FileName) Then  'ファイル名に禁止文字が含まれている
        //        IsValidFileName = False
        //    End If
        //
        //    Set hFso = Nothing
        //
        //End Function
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
        #endregion
        #endregion

        #region ファイル名の禁止文字のチェック(\,/,:,*,?,",<,>,|)(ピリオド[.]判定なし)
        //********************************************************************************
        //機    能  ：  ファイル名の禁止文字のチェック(\,/,:,*,?,",<,>,|)(ピリオド[.]判定なし)
        //              変数名           [I/O] 型        内容
        //引    数  ：  FileName         [I/O] String    文字列
        //戻 り 値  ：                   [ /O] Boolean   判定結果  True:可  False:不可
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        public static bool FileNameProhibitionCheck2(string FileName)
        {
            bool functionReturnValue = false;

            //ファイル名の禁止文字
            const string ProhibitStr = "\\/:*?<>| " + "\"";

            //戻り値初期化
            int i = 0;
            functionReturnValue = false;

            for (i = 1; i <= FileName.Length; i++)
            {
                if (ProhibitStr.IndexOf(FileName.Substring(i - 1, 1)) > -1) return functionReturnValue;
            }

            functionReturnValue = true;
            return functionReturnValue;
        }
        #endregion

        #region ディレクトリ名およびファイル名のチェック関数(ピリオド判定なし)
        //*******************************************************************************
        //機　　能： ディレクトリ名およびファイル名のチェック関数(ピリオド判定なし)
        //
        //           変数名          [I/O] 型        内容
        //引　　数： DirName         [I/ ] String
        //
        //戻 り 値：                 [ /O] Boolean
        //
        //補　　足： なし
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
        public static bool IsValidFileName2(string DirName, string FileName)
        {

            #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
            /*
			Dim hFso As FileSystemObject
			Set hFso = New FileSystemObject
            */
            #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            //v8.0 削除 by 村田
            //戻り値初期化
            //    IsValidFileName = False
            //
            //    If DirName = "" Then                                'ディレクトリ名を指定していない
            //        Exit Function
            //    ElseIf Dir(DirName, vbDirectory) = "" Then          'ディレクトリが存在しない
            //        Exit Function
            //    ElseIf fileName = "" Then                           'ファイル名を指定していない
            //        Exit Function
            //    ElseIf Not FileNameProhibitionCheck(fileName) Then  'ファイル名に禁止文字が含まれている
            //        Exit Function
            //    End If
            //
            //    IsValidFileName = True

            //戻り値初期化
            bool functionReturnValue = true;

            //フォルダの存在確認
            if (!Directory.Exists(DirName))
            {
                functionReturnValue = false;
            }
            //ファイルの存在確認
            else if (!File.Exists(Path.Combine(DirName, FileName)))
            {
                functionReturnValue = false;
            }
            else if (!FileNameProhibitionCheck2(FileName))		//ファイル名に禁止文字が含まれている
            {
                functionReturnValue = false;
            }

            #region CT30Kv19.13_64bit 化不要コメントアウト_完全版
            /*
			Set hFso = Nothing
            */
            #endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

            return functionReturnValue;
        }
        #endregion

        //<- v19.00

    }
}