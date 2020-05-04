// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const data = {
    nodes: [
        {
            id: 'node1',
            x: 100,
            y: 200,
        },
        {
            id: 'node2',
            x: 300,
            y: 200,
        },
        {
            id: 'node3',
            x: 300,
            y: 300,
            label: 'Hello my name is',
        },
    ],
    edges: [
        {
            id: 'edge1',
            target: 'node2',
            source: 'node1',
        },
    ],
};

//const width = document.getElementById('container').;
//const height = document.getElementById('container')
fetch('/Home/GetResult')
    .then(res => res.json())
    .then(data => {
        const graph = new G6.Graph({
            container: 'container',
            width: 1920,
            height: screen.height,
            layout: {
                type: 'mds',
                linkDistance: 300,
            },
            modes: {
                default: ['drag-node', 'drag-canvas',
                    'zoom-canvas'],
            },
            defaultNode: {
                size: 70,
                style: {
                    fill: '#C6E5FF',
                    stroke: '#5B8FF9',
                },
            },
            defaultEdge: {
                size: 1,
                color: '#e2e2e2',
            },
        });
        graph.data(data);
        graph.render();
    });
