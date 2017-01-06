using SeatAssignment.Entities;
using SeatAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatAssignment.BusinessLogic
{
    /// <summary>
    /// Sorts reservation requests to assign contiguous seats to smaller groups on priority
    /// </summary>
    public class SmallGroupFavoringTheaterManager : FairTheaterManager
    {
        public SmallGroupFavoringTheaterManager() : base()
        { }

        public override List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests)
        {
            var sortedRequests = requests.OrderBy(request => request.NumberOfSeats).ToList();
            return base.AssignSeats(sortedRequests);
        }
    }
}