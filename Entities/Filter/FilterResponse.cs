using System.Collections.Generic;

namespace stake_place_web.Entities.Filter
{
    public class FilterResponse
    {
        public FilterResponse(){
            _viewLines = ViewLine.GetAllLines;
        }
        private List<ViewLine> _viewLines;
        private List<MarketLine> _marketLines;
        private List<RecordLine> _recordLines;
        private List<SportLine> _sportLines;
        private List<TransactionLine> _transactionLines;
        private List<VipLine> _vipLines;
        private List<SpecialLine> _specialLines;
        private List<TicketLine> _ticketLines;
        private List<StatusLine> _statusLines;
    }
}