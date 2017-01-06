using SeatAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatAssignment.Entities;

namespace SeatAssignment.BusinessLogic
{
    /// <summary>
    /// Sorts reservation requests to assign contiguous seats to larger groups on priority
    /// </summary>
    public class LargeGroupFavoringTheaterManager : FairTheaterManager
    {
        public LargeGroupFavoringTheaterManager() : base()
        { }

        public override List<ReservationAssignment> AssignSeats(List<ReservationRequest> requests)
        {
            var sortedRequests = requests.OrderByDescending(request => request.NumberOfSeats).ToList();
            return base.AssignSeats(sortedRequests);
        }
    }
}
