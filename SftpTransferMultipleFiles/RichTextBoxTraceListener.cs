using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentPro.Diagnostics;

namespace SftpSamples
{
    public class RichTextBoxTraceListener : UltimateTextWriterTraceListener
    {
        private int _maxCharsCount;

        private static readonly Color TextColorCommand = Color.White; // Text color for command texts.
        private static readonly Color TextColorError = Color.FromArgb(0xff, 0x50, 0x50); // Text color for error texts.
        internal static readonly Color TextColorInfo = Color.FromArgb(0x72, 0xff, 0x7c); // Text color for information texts.
        private static readonly Color TextColorResponse = Color.FromArgb(0xa0, 0xa0, 0xa0); // Text color for response texts.
        private static readonly Color TextColorSecure = Color.FromArgb(0x8b, 0xf5, 0xfc); // Text color for security information texts.
        private readonly RichTextBox _textbox;

        public RichTextBoxTraceListener(RichTextBox textbox)
            : this(textbox, 1000000)
        {
        }

        public RichTextBoxTraceListener(RichTextBox textbox, int maxCharsCount)
        {
            _textbox = textbox;
            _maxCharsCount = maxCharsCount;
        }

        public override void TraceData(object source, TraceEventType level, string category, string message)
        {
            Color color = TextColorInfo;

            string prefix = string.Format("[{0:HH:mm:ss.fff}] {1}", DateTime.Now, source);
            string body;

            // If it's showing an error?
            if (level <= TraceEventType.Error)
            {
                color = TextColorError;
            }
            else
            {
                switch (category.ToUpper())
                {
                    case "COMMAND":
                        // command log.
                        color = TextColorCommand;
                        body = string.Format(" {0} - COMMAND>   ", level);
                        goto Invoke;

                    case "RESPONSE":
                        // response log.
                        color = TextColorResponse;
                        body = string.Format(" {0} -        <   ", level);
                        goto Invoke;

                    case "SECURESHELL":
                    case "SECURESOCKET":
                        color = TextColorSecure;
                        break;
                }
            }
            body = string.Format(" {0} - {1}: ", level, category);

        Invoke:
            try
            {
                if (!_textbox.IsDisposed)
                    _textbox.Invoke(new OnLogHandler(OnLog), new object[] { prefix + body + message + "\r\n", color });
            }
            catch (ObjectDisposedException)
            {
            }
        }


        private void OnLog(string message, Color color)
        {
            EnsureCaretPosition(message.Length);

            _textbox.Focus();
            _textbox.SelectionColor = color;
            _textbox.AppendText(message);
        }

        private void EnsureCaretPosition(int length)
        {
            if (_textbox.TextLength + length < _maxCharsCount)
                return;

            int spaceLeft = _maxCharsCount - length;

            if (spaceLeft <= 0)
            {
                _textbox.Clear();
                return;
            }

            string plainText = _textbox.Text;

            // Find the end of line
            int start = plainText.IndexOf('\n', plainText.Length - spaceLeft);
            if (start >= 0 && start + 1 < plainText.Length)
            {
                _textbox.SelectionStart = 0;
                _textbox.SelectionLength = start + 1;

                // Setting the SelectedText property is available only when ReadOnly = false
                bool isReadOnly = _textbox.ReadOnly;
                _textbox.ReadOnly = false;
                _textbox.SelectedText = "";
                _textbox.ReadOnly = isReadOnly;

                _textbox.SelectionStart = _textbox.TextLength;
                _textbox.SelectionLength = 0;
            }
            else
            {
                _textbox.Clear();
            }
        }

        #region Nested type: OnLogHandler

        private delegate void OnLogHandler(string message, Color color);

        #endregion
    }
}