#if UNITY_EDITOR

using UnityEditor.Experimental.GraphView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    public class TreeStartNode : Node
    {
        public Port outputPort;
        public TreeStartNode()
        {
            title = "Start";

            // Output 포트 추가
            outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputPort.portName = "Output";
            outputContainer.Add(outputPort);

            RefreshExpandedState();
            RefreshPorts();
        }
    }
}

#endif
