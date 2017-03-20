using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SEC.Nanoeye.NanoeyeSEM
{
    public class ProgressCheckingEventArgs : EventArgs
    {
        private int m_Progress = 0;
        private bool m_Cancel = false;
        private long m_Elapsed = 0;

        /// <summary>
        /// 작업이 시작된 후의 밀리초 경과시간 입니다.
        /// </summary>
        public long Elapsed
        {
            get { return m_Elapsed; }
        }

       /// <summary>
       /// 작업을 취소하는 값을 지정합니다.
       /// </summary>
       public bool Cancel
        {
            get { return m_Cancel; }
            set { m_Cancel = value; }
        }

        /// <summary>
        /// 진행율을 지정합니다. 0 : 100의 정수입니다.
        /// </summary>
        public int Progress
        {
            get { return m_Progress; }
            set { m_Progress = value; }
        }

        public ProgressCheckingEventArgs(long elapsed)
        {
        }
    }
}
