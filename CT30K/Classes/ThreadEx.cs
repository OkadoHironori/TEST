using System;
using System.Threading;

namespace CT30K
{
	/// <summary>
	/// �X���b�h���쐬����ѐ��䂵�A���̃X���b�h�̗D�揇�ʂ̐ݒ肨��уX�e�[�^�X�̎擾���s���B
	/// </summary>
	public class ThreadEx
	{
		#region Fields

		private Thread _Thread = null;
		private volatile bool _Stoped = false;
		private ManualResetEvent _Event = new ManualResetEvent(false);

		#endregion Fields

		#region Constructors

		/// <summary>
		/// ThreadEx �N���X�̐V�����C���X�^���X������������B 
		/// </summary>
		/// <param name="start"></param>
		public ThreadEx(ThreadStart start)
		{
			_Thread = new Thread(start);
		}

		/// <summary>
		/// �X���b�h�̊J�n���ɃI�u�W�F�N�g���X���b�h�ɓn�����Ƃ�������f���Q�[�g���w�肵�āA�V�����C���X�^���X������������B 
		/// </summary>
		/// <param name="start"></param>
		public ThreadEx(ParameterizedThreadStart start)
		{
			_Thread = new Thread(start);
		}

		#endregion Constractors

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get { return _Thread.Name; }
			set { _Thread.Name = value; }
		}

		/// <summary>
		/// �X���b�h�̃X�P�W���[�����O�D�揇�ʂ������l���擾�܂��͐ݒ肷��B 
		/// </summary>
		public ThreadPriority Priority
		{
			get { return _Thread.Priority; }
			set { _Thread.Priority = value; }
		}

		/// <summary>
		/// �X���b�h���~���邩�ǂ����������l���擾�܂��͐ݒ肷��B
		/// </summary>
		public bool Stoped
		{
			get { return _Stoped; }
			private set { _Stoped = value; }
		}

		/// <summary>
		/// ���݂̃X���b�h�̏�Ԃ������l���擾����B 
		/// </summary>
		public ThreadState ThreadState
		{
			get { return _Thread.ThreadState; }
		}

		/// <summary>
		/// ���݂̃X���b�h�̎��s�X�e�[�^�X�������l���擾����B
		/// </summary>
		public bool IsAlive
		{
			get { return _Thread.IsAlive; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// �I�y���[�e�B���O �V�X�e���ɂ���āA���݂̃C���X�^���X�̏�Ԃ� ThreadState.Running �ɕύX����B 
		/// </summary>
		public void Start()
		{
			_Thread.Start();
		}

		/// <summary>
		/// �I�y���[�e�B���O �V�X�e���ɂ���Č��݂̃C���X�^���X�̏�Ԃ� ThreadState.Running �ɕύX����A�I�v�V�����ŃX���b�h�����s���郁�\�b�h�Ŏg�p����f�[�^���i�[����I�u�W�F�N�g���񋟂����B
		/// </summary>
		public void Start(object parameter)
		{
			_Thread.Start(parameter);
		}
		
		/// <summary>
		/// ���̃��\�b�h���Ăяo���ꂽ�Ώۂ̃X���b�h���I������B 
		/// </summary>
		public void Stop()
		{
			_Event.Set();

			this.Stoped = true;

			_Thread.Join(2000);
		}

		/// <summary>
		/// ���̃��\�b�h���Ăяo���ꂽ�Ώۂ̃X���b�h�ŁA���̃X���b�h�̏I���v���Z�X���J�n���� ThreadAbortException �𔭐�������B
		/// </summary>
		public void Abort()
		{
			try
			{
				_Thread.Abort();
			}
			catch
			{
			}
		}

		/// <summary>
		/// �w�肵�����Ԃ������݂̃X���b�h�𒆒f���邪�A�X���b�h����~�����ꍇ�͒��f���瑦���ɕ������A��~����邩�ǂ����������l��Ԃ��B
		/// </summary>
		/// <param name="millisecondsTimeout">�X���b�h���u���b�N�����~���b���B</param>
		/// <returns>�X���b�h����~�����ꍇ�� true�B����ȊO�� false�B</returns>
		public bool Sleep(int millisecondsTimeout)
		{
			return _Event.WaitOne(millisecondsTimeout);
		}

		/// <summary>
		/// �w�肵�����Ԃ������݂̃X���b�h�𒆒f���邪�A�X���b�h����~�����ꍇ�͒��f���瑦���ɕ������A��~����邩�ǂ����������l��Ԃ��B
		/// </summary>
		/// <param name="millisecondsTimeout">�X���b�h���u���b�N����鎞�Ԃɐݒ肳��� TimeSpan�B</param>
		/// <returns>�X���b�h����~�����ꍇ�� true�B����ȊO�� false�B</returns>
		public bool Sleep(TimeSpan timeout)
		{
			return _Event.WaitOne(timeout);
		}

		#endregion Methods
	}
}
