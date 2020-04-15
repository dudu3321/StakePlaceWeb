import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import TabContent from '../TabContent/TabContent';
import TabFilter from '../TabFilter/TabFilter';
import TabInfo from '../TabInfo/TabInfo';
import { DndProvider, DragSource, DropTarget } from 'react-dnd';
import HTML5Backend from 'react-dnd-html5-backend';
import { Tabs } from 'antd';
import './Main.styles.scss';

const { TabPane } = Tabs;

class Main extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      panes: [{ title: 'Tab #1', key: '1' }],
      newTabIndex: 1,
      activeKey: '1',
    };
  }

  filterEventHandler = (filterState) => {
      
  }

  eventHandler = (targetKey, action) => {
    this[action](targetKey);
  };

  change = (activeKey) => {
    this.setState({ activeKey });
  };

  add = () => {
    const { panes } = this.state;
    const activeKey = (++this.state.newTabIndex).toString();
    panes.push({
      title: `Tab #${this.state.newTabIndex}`,
      key: activeKey,
    });
    this.setState({ panes, activeKey });
  };

  remove = (targetKey) => {
    let { activeKey } = this.state;
    let lastIndex;
    this.state.panes.forEach((pane, i) => {
      if (pane.key === targetKey) {
        lastIndex = i - 1;
      }
    });
    const panes = this.state.panes.filter((pane) => pane.key !== targetKey);
    if (panes.length && activeKey === targetKey) {
      if (lastIndex >= 0) {
        activeKey = panes[lastIndex].key;
      } else {
        activeKey = panes[0].key;
      }
    }
    this.setState({ panes, activeKey });
  };

  render() {
    return (
      <div className="tab_content">
        <DraggableTabs
          handler={this.eventHandler}
          activeKey={this.state.activeKey}
        >
          {this.state.panes.map((pane) => {
            return (
              <TabPane tab={pane.title} key={pane.key}>
                <div className="tab_content_control">
                  <TabFilter></TabFilter>
                </div>
                <div className="tab_content_control">
                  <TabInfo></TabInfo>
                </div>
                <div className="tab_content_control">
                  <TabContent></TabContent>
                </div>
              </TabPane>
            );
          })}
        </DraggableTabs>
      </div >
    );
  }
}

// Drag & Drop node
class TabNode extends React.Component {
  render() {
    const { connectDragSource, connectDropTarget, children } = this.props;

    return connectDragSource(connectDropTarget(children));
  }
}

const cardTarget = {
  drop(props, monitor) {
    const dragKey = monitor.getItem().index;
    const hoverKey = props.index;

    if (dragKey === hoverKey) {
      return;
    }

    props.moveTabNode(dragKey, hoverKey);
    monitor.getItem().index = hoverKey;
  },
};

const cardSource = {
  beginDrag(props) {
    return {
      id: props.id,
      index: props.index,
    };
  },
};

const WrapTabNode = DropTarget('DND_NODE', cardTarget, (connect) => ({
  connectDropTarget: connect.dropTarget(),
}))(
  DragSource('DND_NODE', cardSource, (connect, monitor) => ({
    connectDragSource: connect.dragSource(),
    isDragging: monitor.isDragging(),
  }))(TabNode)
);

class DraggableTabs extends React.Component {
  state = {
    order: [],
  };

  onEdit = (targetKey, action) => {
    const { handler } = this.props;
    handler(targetKey, action);
  };

  onChange = (activeKey) => {
    this.onEdit(activeKey, 'change');
  };

  moveTabNode = (dragKey, hoverKey) => {
    const newOrder = this.state.order.slice();
    const { children } = this.props;

    React.Children.forEach(children, (c) => {
      if (newOrder.indexOf(c.key) === -1) {
        newOrder.push(c.key);
      }
    });

    const dragIndex = newOrder.indexOf(dragKey);
    const hoverIndex = newOrder.indexOf(hoverKey);

    newOrder.splice(dragIndex, 1);
    newOrder.splice(hoverIndex, 0, dragKey);

    this.setState({
      order: newOrder,
    });
  };

  renderTabBar = (props, DefaultTabBar) => (
    <DefaultTabBar {...props}>
      {(node) => (
        <WrapTabNode
          key={node.key}
          index={node.key}
          moveTabNode={this.moveTabNode}
        >
          {node}
        </WrapTabNode>
      )}
    </DefaultTabBar>
  );

  render() {
    const { order } = this.state;
    const { children } = this.props;

    const tabs = [];
    React.Children.forEach(children, (c) => {
      tabs.push(c);
    });

    const orderTabs = tabs.slice().sort((a, b) => {
      const orderA = order.indexOf(a.key);
      const orderB = order.indexOf(b.key);

      if (orderA !== -1 && orderB !== -1) {
        return orderA - orderB;
      }
      if (orderA !== -1) {
        return -1;
      }
      if (orderB !== -1) {
        return 1;
      }

      const ia = tabs.indexOf(a);
      const ib = tabs.indexOf(b);

      return ia - ib;
    });

    return (
      <DndProvider backend={HTML5Backend}>
        <Tabs
          type="editable-card"
          onEdit={this.onEdit}
          onChange={this.onChange}
          activeKey={this.state.activeKey}
          renderTabBar={this.renderTabBar}
          {...this.props}
        >
          {orderTabs}
        </Tabs>
      </DndProvider>
    );
  }
}

Main.propTypes = {
  // bla: PropTypes.string,
};

Main.defaultProps = {
  // bla: 'test',
};

export default Main;
