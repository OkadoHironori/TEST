using System;

namespace CT30K
{
    internal static class CTResources
	{
		/// <summary>
		/// 指定した id の文字列をリソースから取得する。
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string LoadResString(int id)
		{
            string s = string.Format("_{0}", id);
            return Properties.Resources.ResourceManager.GetString(s);
		}
	}
}
