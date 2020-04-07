import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import TabContent from '../TabContent/TabContent';
import TabFilter from '../TabFilter/TabFilter';
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import { Tabs } from 'antd';

import './Main.styles.scss';

const { TabPane } = Tabs;


class Main extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      panes: [{ title: 'Tab #1', key: '1' }],
      newTabIndex: 1,
      activeKey: '1'
    };
  }

  onDragEnd = result => {
    const { source, destination } = result;
    if (!destination) {
      return;
    }

    let arr = Array.from(this.state.panes);
    const [remove] = arr.splice(source.index, 1);
    arr.splice(destination.index, 0, remove);
    this.setState({
      panes: arr
    });
  };

  onChange = activeKey => {
    this.setState({ activeKey });
  };
  onEdit = (targetKey, action) => {
    this[action](targetKey);
  };

  add = () => {
    const { panes } = this.state;
    const activeKey = (++this.state.newTabIndex).toString();
    panes.push({
      title: `Tab #${this.state.newTabIndex}`,
      key: activeKey
    });
    this.setState({ panes, activeKey });
  };

  remove = targetKey => {
    let { activeKey } = this.state;
    let lastIndex;
    this.state.panes.forEach((pane, i) => {
      if (pane.key === targetKey) {
        lastIndex = i - 1;
      }
    });
    const panes = this.state.panes.filter(pane => pane.key !== targetKey);
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
      <div className="Content">
        <Tabs
          type="editable-card"
          onEdit={this.onEdit}
          activeKey={this.state.activeKey}
          onChange={this.onChange}
        >
          <DragDropContext onDragEnd={this.onDragEnd}>
            <Droppable droppableId="droppable-1">
              {provided => (
                <div ref={provided.innerRef} {...provided.droppableProps}>
                  {this.state.panes.map((pane, index) => (
                    <Draggable draggableId={pane.key} index={index}>
                      {p => (
                        <TabPane
                          tab={pane.title}
                          key={pane.key}
                          closable={true}
                          ref={provided.innerRef}
                          {...provided.draggableProps}
                          {...provided.dragHandleProps}
                        >
                          <TabFilter></TabFilter>
                          <TabContent></TabContent>
                        </TabPane>
                      )}
                    </Draggable>
                  ))}
                  {provided.placeholder}
                </div>
              )}
            </Droppable>
          </DragDropContext>
        </Tabs>
      </div>
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
