//-----------------------------------------------------------------------------
// @(s)
// ITC���C�u���� [�t�@�C��]
//
// @(h) ���O�t�@�C���������� ( '05/07/25 (ITC)(SI3)K.Yahagi )
//
//  (c) Copyright 2005 by Toshiba IT & Control Systems Corporation
//-----------------------------------------------------------------------------
// @(h) ���e�ύX����
// (�C�����e ���t �ύX�Җ� <�C���^�O>)
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace CT30K.Common.Library
{
    /// <summary>
    /// ���O���x��
    /// </summary>
    public enum LogLevel
    {
        // �D��x����������
        /// <summary>
        /// �G���[
        /// </summary>
        Error,

        /// <summary>
        /// �x��
        /// </summary>
        Warning,

        /// <summary>
        /// ���
        /// </summary>
        Information,

        /// <summary>
        /// �g���[�X
        /// </summary>
        Trace
    }

    /// <summary>
    /// �G���[���O�N���X
    /// </summary>
    public class ErrLogger
    {
        private const string LOG_FOLDER = @"Log\Err";		// �G���[���O�t�H���_
        private const string LOG_PREFIX = @"Err_";		// �G���[���O�t�H���_

        private static LogLevel logLevel = LogLevel.Trace;  // �o�̓��O���x��
        private static Logger logging = new Logger();       // ���O�N���X

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        private ErrLogger()
        {
        }

        /// <summary>
        /// ���O���x���擾�A�ݒ�
        /// </summary>
        public static LogLevel Level
        {
            get { return logLevel; }
            set
            {
                if (value >= LogLevel.Warning)
                {
                    logLevel = value;
                }
                else
                {
                    logLevel = LogLevel.Warning;
                }
            }
        }

        /// <summary>
        /// ���O������
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="logs">�o�͕�����i�����w��\�j</param>
        public static void Write(LogLevel level, int stack, params string[] logs)
        {
            // �o�͗D��x���Ⴂ(�l���傫��)�ꍇ�͉������Ȃ�
            if (logLevel < level)
            {
                return;
            }

            string levelLog = "";
            switch (level)
            {
                case LogLevel.Error:
                    levelLog = "[�G���[]";
                    break;
                case LogLevel.Warning:
                    levelLog = "[�x��]";
                    break;
                case LogLevel.Information:
                    levelLog = "[���]";
                    break;
                case LogLevel.Trace:
                    levelLog = "[�g���[�X]";
                    break;
            }

            string folder = Path.Combine(logging.AppDir, LOG_FOLDER);

            // ���݂̓��t
            DateTime now = DateTime.Now;
            string nowDate = now.ToString(Logger.DATE_FILENAME);
            string filename = LOG_PREFIX + nowDate + Logger.LOG_EXT;

            // �G���[�����\�b�h���擾
            StackTrace trace = new StackTrace(0);
            if (stack >= trace.FrameCount)
            {
                stack = trace.FrameCount - 1;
            }
            if (stack < 1)
            {
                stack = 1;
            }
            StackFrame frame = trace.GetFrame(stack);
            MethodBase method = frame.GetMethod();
            StringBuilder baseName = new StringBuilder(method.ReflectedType.Name);
            baseName.AppendFormat(".{0}()", method.Name);

            // ���O������쐬
            string[] logArray = new string[logs.Length + 2];
            logArray[0] = levelLog;
            logArray[1] = baseName.ToString();
            logs.CopyTo(logArray, 2);

            // ���O�ǉ�
            logging.AddLog(folder, filename, logArray);

        }

        /// <summary>
        /// ���O�t�@�C���폜
        /// </summary>
        /// <param name="keepCount">���O�t�@�C���̕ێ���</param>
        public static void CleanUp(int keepCount)
        {
            //���O�폜
            logging.DeleteLog(Path.Combine(logging.AppDir, LOG_FOLDER), keepCount);
        }
    }
}
