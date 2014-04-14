using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iisu;
using Iisu.Data;
using Retargeting;

namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {

            try
            {
           
                //Instantiates Iisu engine and loads a movie(root path -> default Iisu installation).
                IisuWrapper iisu = new IisuWrapper();

                //Iisu wrapper runs until the end of the input movie.
                while (iisu.isRunning())
                {
                    //Acquires a frame from iisu
                    iisu.updateFrame();

                    //copy Iisu skeleton keypoints into a Retargeting object(SourcePose).
                    //SourcePose sourcePose = Utility.Converter(iisu.keypoints);

                    //if (!sourcePose.IsComplete)
                    //{
                    //    Console.WriteLine("{0} are missing in the sourcePose object !", sourcePose.CountMissingKeypoints);
                    //    return;
                    //}

                    //Provides keypoints to the Retargeting engine
                    //character.setSourceKeypoints(sourcePose.toArray());

                    //Starts the Retargeting process.
                    //character.retarget();

                    //Console.WriteLine("Retargeting results: ");

                    //String[] jointNames = character.targetJointNamesOrder;
                    //int index = 0;
                    //foreach (ScaledFrame f in character.targetFrames)
                    //{
                    //    Console.WriteLine("Joint Name  : {0}", jointNames[index]);
                    //    Console.WriteLine("Position    : [x={0} , y={1} , z={2} ]", f.position.x, f.position.y, f.position.z);
                    //    Console.WriteLine("Orientation : [x={0} , y={1} , z={2} ,w={3} ]", f.quaternion.x, f.quaternion.y, f.quaternion.z, f.quaternion.w);
                    //    Console.WriteLine("Scale       : {0}", f.scale);

                    //    ++index;
                    //}

                    //Releases the acquired frame from iisu
                    iisu.releaseFrame();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
