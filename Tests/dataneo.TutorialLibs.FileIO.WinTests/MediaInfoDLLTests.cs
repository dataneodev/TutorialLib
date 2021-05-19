using MediaInfo.DotNetWrapper.Enumerations;
using System;
using Xunit;

namespace dataneo.TutorialLibs.FileIO.WinTests
{
    public class MediaInfoDLLTests
    {
        [Fact]
        public void LoadFileTest()
        {
            try
            {
                string text = null;
                using (var mediaInfo = new MediaInfo.DotNetWrapper.MediaInfo())
                {
                    text += "\r\n\r\nOpen\r\n";
                    mediaInfo.Open(@"F:\Filmy na dysku\12 Strong (2018).mp4");

                    text += "\r\n\r\nInform with Complete=false\r\n";
                    mediaInfo.Option("Complete");
                    text += mediaInfo.Inform();

                    text += "\r\n\r\nInform with Complete=true\r\n";
                    mediaInfo.Option("Complete", "1");
                    text += mediaInfo.Inform();

                    text += "\r\n\r\nCustom Inform\r\n";
                    mediaInfo.Option("Inform", "General;File size is %FileSize% bytes");
                    text += mediaInfo.Inform();

                    foreach (string param in new[] { "BitRate", "BitRate/String", "BitRate_Mode" })
                    {
                        text += "\r\n\r\nGet with Stream=Audio and Parameter='" + param + "'\r\n";
                        text += mediaInfo.Get(StreamKind.Audio, 0, param);
                    }

                    text += "\r\n\r\nGet with Stream=General and Parameter=46\r\n";
                    text += mediaInfo.Get(StreamKind.General, 0, 46);

                    text += "\r\n\r\nCount_Get with StreamKind=Stream_Audio\r\n";
                    text += mediaInfo.CountGet(StreamKind.Audio);

                    text += "\r\n\r\nGet with Stream=General and Parameter='AudioCount'\r\n";
                    text += mediaInfo.Get(StreamKind.General, 0, "AudioCount");

                    text += "\r\n\r\nGet with Stream=Audio and Parameter='StreamCount'\r\n";
                    text += mediaInfo.Get(StreamKind.Audio, 0, "StreamCount");

                    text += "\r\n\r\nGK 1\r\n";
                    text += mediaInfo.Get(StreamKind.General, 0, "FileSize");

                    text += "\r\n\r\nGK 2\r\n";
                    text += mediaInfo.Get(StreamKind.General, 0, "Duration");
                }

            }
            catch (Exception e)
            {

            }
        }
    }
}
