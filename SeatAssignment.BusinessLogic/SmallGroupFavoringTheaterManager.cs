using SeatAssignment.Entities;
using SeatAssignment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatAssignment.BusinessLogic
{
    public class SmallGroupFavoringTheaterManager : FairTheaterManager, ITheaterManager
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