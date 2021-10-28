//Author: Shreyas Agrawal
//Date completed: 24 Jul 2019
//Part of CLINIS internship

namespace Scheduled_pinging_of_IP_address_and_message_on_failure_to_slack
{
    class Program
    {
        static void Main(string[] args)
        {
            var scheduler = new Scheduler();
            scheduler.Start(30); //the interval after which the program runs again

        }

    }
}
