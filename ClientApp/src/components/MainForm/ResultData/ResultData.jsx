import React, { PureComponent } from 'react'
import { withCookies, Cookies } from 'react-cookie';
import { Table } from 'antd';

class ResultData extends PureComponent {
  constructor(props) {
    super(props)
    const { selectedFilters, cookies } = this.props;
    this.state = {
      userLevels: cookies['userLevels'] ?? '',
      selectedFilters: selectedFilters,
      resultData: [],
      lastUpdateTime: ''
    };
    // setInterval(this.getResultData, 10000);
    // this.getResultData();
  }

  getResultData = () => {
    if (Object.keys(this.state.selectedFilters).length === 0) return;

    const { contentEventHandler } = this.props;
    fetch('ticket', {
      method: 'POST',
      headers: {
        'content-type': 'application/json'
      },
      body: JSON.stringify({
        ...this.state.selectedFilters,
        userLevels: this.state.userLevels,
        lastUpdateTime: this.state.lastUpdateTime
      })
    })
      .then(response => response.json())
      .then(result => {
        let newResultData = Object.assign({}, this.resultData);
        let latePendingTicket;
        if (result) {
          //更新舊有資料
          // result.forEach(e => {
          //   let oldElement = newResultData.find(x => x.SocTransId == e.SocTransId);
          //   if (oldElement) {
          //     oldElement = e;
          //   }
          //   else {
          //     newResultData.push(e);
          //   }
          // });

          //更新latePending
          // contentEventHandler(result.reduce((a, b) => a < b ? a : b));

          this.setState({
            resultData: newResultData,
            lastUpdateTime: Date.now()
          });
        }
      });
  }

  render() {
    return (
      <Table columns={columns} ></Table>
      // dataSource={this.state.resultData}
    )
  }
}

const columns = [
  {
    title: 'Account',
    dataIndex: 'Account',
    key: 'Account',
  },
  {
    title: 'League',
    dataIndex: 'League',
    key: 'League',
  },
  {
    title: 'Home',
    dataIndex: 'Home',
    key: 'Home',
  },
  {
    title: 'Away',
    dataIndex: 'Away',
    key: 'Away',
  },
  {
    title: 'Type',
    dataIndex: 'Type',
    key: 'Type',
  },
  {
    title: 'Run',
    dataIndex: 'Run',
    key: 'Run',
  },
  {
    title: 'HDP',
    dataIndex: 'HDP',
    key: 'HDP',
  },
  {
    title: 'MMR',
    dataIndex: 'MMR',
    key: 'MMR',
  },
  {
    title: 'Odds',
    dataIndex: 'Odds',
    key: 'Odds',
  },
  {
    title: 'MY',
    dataIndex: 'MY',
    key: 'MY',
  },
  {
    title: 'Amt',
    dataIndex: 'Amt',
    key: 'Amt',
  },
  {
    title: 'D',
    dataIndex: 'D',
    key: 'D',
  },
  {
    title: 'Time',
    dataIndex: 'Time',
    key: 'Time',
  },
  {
    title: 'Date',
    dataIndex: 'Date',
    key: 'Date',
  },
  {
    title: 'IP',
    dataIndex: 'IP',
    key: 'IP',
  },
  {
    title: 'Ref',
    dataIndex: 'Ref',
    key: 'Ref',
  },
  {
    title: 'Operated',
    dataIndex: 'Operated',
    key: 'Operated',
  },
  {
    title: 'Updated',
    dataIndex: 'Updated',
    key: 'Updated',
  },
  {
    title: 'Code',
    dataIndex: 'Code',
    key: 'Code',
  }
];

export default withCookies(ResultData);