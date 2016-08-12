using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleApplication
{
    //contains road state
    class Road
    {
        public bool vehiclePresent;
        public bool light;
        public int priority;
        public string direction;
        

        public Road(string dir, Random randomGenerator)
        {
            direction = dir;
            vehiclePresent = (randomGenerator.Next(2) == 0);
            light = false;
            priority = 0;
        }

        public void getNewVehiclePresentState(Random randomGenerator)
        {
            vehiclePresent = (randomGenerator.Next(2) == 0);
        }
        public void incrementPriority()
        {
            priority += 1;
        }
    }

    class FourWayIntersectionSimulation
    {
        public static Road[] intersection = new Road[4];
        public static readonly Random rnd = new Random();

        static void initializeIntersection()
        {
            string[] directions = new string[] {"North","East","South","West" };
            for(int i = 0;i<intersection.Length;i++)
            {
                Road newRoad = new Road(directions[i],rnd);
                intersection[i] = newRoad;
            }
        }

        static void printIntersectionState()
        {
            for(int i = 0; i<intersection.Length; i++)
            {
                string vPreseneceString;
                string lightColourString;

                vPreseneceString = (intersection[i].vehiclePresent) ? "Vehicle Present": "Vehicle Not Present";

                lightColourString = (intersection[i].light) ? "Green - GO!" : lightColourString = "Red";

                Console.WriteLine(intersection[i].direction + ": \n\t=>" + vPreseneceString + "\n\t=>" + lightColourString + "\n\t=>Priority:\n\t\t" + intersection[i].priority);
            }
            Console.WriteLine("============\n\n");
        }

        //return the highest priority value in the intersection
        static int getHighestPriority()
        {
            int highest = 0;
            for(int i = 0; i<intersection.Length; i++)
            {
                if(intersection[i].priority > highest)
                {
                    highest = intersection[i].priority;
                }
            }
            return highest;
        }

        static void Main()
        {
            initializeIntersection();

            Console.WriteLine("Four Way Intersection Simulation.  \'q\' + enter to exit program.\n\n");

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(updateState);
            aTimer.Interval = 10000;
            aTimer.Enabled = true;
            while (Console.Read() != 'q') ;

        }

        //set vehicle to moving into intersection
        static void Go(Road throughVehicle)
        {
            throughVehicle.getNewVehiclePresentState(rnd);
            throughVehicle.light = true;
            throughVehicle.priority = 0;
        }

        private static void updateState(object source, ElapsedEventArgs e)
        {
            printIntersectionState();
            int hp = getHighestPriority();
            bool clear = true;
            
            for(int i = 0; i<intersection.Length; i++)
            {
                intersection[i].light = false;
                //is there a vehicle present?
                if(intersection[i].vehiclePresent)
                {
                    //are we highest priority?
                    if(intersection[i].priority == hp)
                    {
                        //intersection clear - GO!
                        if (clear)
                        {
                            clear = false;
                            Go(intersection[i]);
                        }
                        //car in intersection - wait
                        else
                        {
                            intersection[i].incrementPriority();
                        }
                    }
                    //were not highest priority - wait
                    else
                    {
                        intersection[i].incrementPriority();
                    }
                }
                //no vehicle present - get new state
                else
                {
                    intersection[i].getNewVehiclePresentState(rnd);
                }
            }
        }
    }
}
