using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iisu;
using Iisu.Data;
using Iisu.PInvoke;
using System.IO;


namespace Retargeting
{
    /// <summary>
    /// Wrapper around Iisu engine.
    /// </summary>
    public class IisuWrapper
    {

        // iisu handle
        private IHandle _iisuHandle;
        private IDevice _device;
        private IDataHandle<int> _Hand1Status;
        private IDataHandle<int> _Hand1PosingGestureId;
        private IDataHandle<bool>  _coincoin;
        private IDataHandle<Iisu.Data.Vector3> _Hand1PalmPosition;
        private IMetaInfo _imetaInfo;
        private int oldHandeStatus= 100;
       
        /// <summary>
        /// Constructor Initializes Iisu and loads the input movie
        /// </summary>
        /// <param name="skvMoviePath">skv movie path</param>
        /// <exception cref="System.Exception">
        /// Can occur when Iisu cannot be properly initialized
        /// or when the input path is not valid.
        /// </exception>
        public IisuWrapper()
        {
            // We need to specify where is located the iisu dll and its configuration file.
            // in this sample we'll use the SDK's environment variable as resource to locate them
            // but you can use any mean you need.
            string libraryLocation = System.Environment.GetEnvironmentVariable("IISU_SDK_DIR");
            IHandleConfiguration config = Iisu.Iisu.Context.CreateHandleConfiguration();
<<<<<<< HEAD


=======
>>>>>>> b8e918927be2a4af4732581fe5ad6d829df34356
            config.IisuBinDir = (libraryLocation + "/bin");
            config.ConfigFileName = "iisu_config.xml";

            // get iisu handle
            _iisuHandle = Iisu.Iisu.Context.CreateHandle(config);
          
            // create iisu device
            _device = _iisuHandle.InitializeDevice();

            // check if Mode DS325 is Enable
            string isCiEnabledString = "";
            _iisuHandle.GetConfigString("//CONFIG//PROCESSING//CI", out isCiEnabledString);
            bool isCiEnabled = isCiEnabledString.Equals("1"); 

            if (isCiEnabled != true)
            {
                Console.WriteLine("Hand Control will not be Ok");
            }

            
            //Envoi du fichier coincoin.iid dans le le moteur IIsi  
            _device.CommandManager.SendCommand("IID.loadGraph", Directory.GetCurrentDirectory() +  "\\coincoin.iid");
        
            // register even listener
            _device.EventManager.RegisterEventListener("SYSTEM.Error", new OnErrorDelegate(onError));
            _Hand1Status = _device.RegisterDataHandle<int>("CI.HAND1.Status");
            _Hand1PosingGestureId = _device.RegisterDataHandle<int>("CI.HAND1.PosingGestureId");
            _Hand1PalmPosition = _device.RegisterDataHandle<Iisu.Data.Vector3>("CI.HAND1.PalmPosition3D");
            
            // enregistrement du la reconnaissance  CoinCoin 
            
            _coincoin = _device.RegisterDataHandle<bool>("IID.Script.CoinCoin");
            _imetaInfo = _device.EventManager.GetMetaInfo("CI.HandPosingGesture");


            _device.Start();
        }

        /// <summary>
        /// Destructor Release Iisu engine.
        /// </summary>
        ~IisuWrapper()
        {
            // destroy the iisu handle
            _iisuHandle.Dispose();
        }

        /// <summary>
        /// Indicates whether the input movie is running or not.
        /// </summary>
        /// <returns>status</returns>
        public bool isRunning()
        {
            return (_device.Status & DeviceStatus.Playing) != 0;
        }

        /// <summary>
        /// Returns the current skeleton keypoints.
        /// </summary>
        /// <remarks>
        /// The method updateFrame should be called before the call of this method.
        /// The method releaseFrame should be called after the call of this method.
        /// </remarks>
        /// <returns>keypoints</returns>
       

        /// <summary>
        ///  Asks Iisu for the last available processed data frame.
        ///  This method waits for skeleton detection.
        /// </summary>
        public void updateFrame()
        {
            _device.UpdateFrame(false);

            if (_Hand1PosingGestureId.Valid)
            {
                if (oldHandeStatus != _Hand1PosingGestureId.Value)
                {
                    IEnumMapper tmp = (IEnumMapper)_imetaInfo.Attributes["ENUM_MAPPER"];
                    Console.WriteLine(tmp.Mapping[(uint)_Hand1PosingGestureId.Value]); 
                    oldHandeStatus = _Hand1PosingGestureId.Value;
                }           
            }
 
            if (_coincoin.Value == true)  
            {
                Console.WriteLine("coincoin ") ; 
            }
        }

        /// <summary>
        /// Releases the frame.
        /// </summary>
        public void releaseFrame()
        {
            _device.ReleaseFrame();
        }

        private delegate void OnErrorDelegate(String name, Iisu.Error e);

        /// <summary>
        ///  Used as a listener of Iisu error events.
        /// </summary>
        /// <param name="e">Error event</param>
        private void onError(String name, Iisu.Error e)
        {
            Console.WriteLine("Error " + e.ErrorCode + ": " + e.Message);
        }
    }
}
