using MediaInfoLib;
using System;
using Xunit;

namespace dataneo.TutorialLibs.FileIO.WinTests
{
    public class MediaInfoDLLTests
    {
        [Fact]
        public void LoadFileTest()
        {
            String ToDisplay;
            MediaInfo MI = new MediaInfo();

            ToDisplay = MI.Option("Info_Version", "0.7.0.0;MediaInfoDLL_Example_CS;0.7.0.0");
            if (ToDisplay.Length == 0)
            {
                Console.Out.WriteLine("MediaInfo.Dll: this version of the DLL is not compatible");
                return;
            }

            //Information about MediaInfo
            ToDisplay += "\r\n\r\nInfo_Parameters\r\n";
            ToDisplay += MI.Option("Info_Parameters");

            ToDisplay += "\r\n\r\nInfo_Capacities\r\n";
            ToDisplay += MI.Option("Info_Capacities");

            ToDisplay += "\r\n\r\nInfo_Codecs\r\n";
            ToDisplay += MI.Option("Info_Codecs");

            //An example of how to use the library
            ToDisplay += "\r\n\r\nOpen\r\n";
            MI.Open(@"‪F:\Filmy na dysku\12 Strong (2018).mp4");

            ToDisplay += "\r\n\r\nInform with Complete=false\r\n";
            MI.Option("Complete");
            ToDisplay += MI.Inform();

            ToDisplay += "\r\n\r\nInform with Complete=true\r\n";
            MI.Option("Complete", "1");
            ToDisplay += MI.Inform();

            ToDisplay += "\r\n\r\nCustom Inform\r\n";
            MI.Option("Inform", "General;File size is %FileSize% bytes");
            ToDisplay += MI.Inform();

            ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='FileSize'\r\n";
            ToDisplay += MI.Get(0, 0, "FileSize");

            ToDisplay += "\r\n\r\nGet with Stream=General and Parameter=46\r\n";
            ToDisplay += MI.Get(0, 0, 46);

            ToDisplay += "\r\n\r\nCount_Get with StreamKind=Stream_Audio\r\n";
            ToDisplay += MI.Count_Get(StreamKind.Audio);

            ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='AudioCount'\r\n";
            ToDisplay += MI.Get(StreamKind.General, 0, "AudioCount");

            ToDisplay += "\r\n\r\nGet with Stream=Audio and Parameter='StreamCount'\r\n";
            ToDisplay += MI.Get(StreamKind.Audio, 0, "StreamCount");

            ToDisplay += "\r\n\r\nClose\r\n";
            MI.Close();
        }
    }
}
