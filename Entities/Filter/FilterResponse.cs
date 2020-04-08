using System.Collections.Generic;
using stake_place_web.Entities.Filter;

namespace stake_place_web.Entities.Filter
{
    public class FilterResponse
    {
        public FilterResponse()
        {
            viewLines = ViewLine.GetAllLines;
            marketLines = MarketLine.GetAllLines;
            recordLines = RecordLine.GetAllLines;
            sportLines = SportLine.GetAllLines;
            transactionLines = TransactionLine.GetAllLines;
            vipLines = VipLine.GetAllLines;
            specialLines = SpecialLine.GetAllLines;
            ticketLines = TicketLine.GetAllLines;
            statusLines = StatusLine.GetAllLines;
        }
        public List<ViewLine> viewLines
        {
            get;
            set;
        }
        public List<MarketLine> marketLines
        {
            get;
            set;
        }
        public List<RecordLine> recordLines
        {
            get;
            set;
        }
        public List<SportLine> sportLines
        {
            get;
            set;
        }
        public List<TransactionLine> transactionLines
        {
            get;
            set;
        }
        public List<VipLine> vipLines
        {
            get;
            set;
        }
        public List<SpecialLine> specialLines
        {
            get;
            set;
        }
        public List<TicketLine> ticketLines
        {
            get;
            set;
        }
        public List<StatusLine> statusLines
        {
            get;
            set;
        }
    }
}