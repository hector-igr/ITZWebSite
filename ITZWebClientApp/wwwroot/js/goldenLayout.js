window.goldenLayout = {
    ini: function (containerName) {
        var config = {
            content: [
                {
                    type: 'row',
                    content: [
                        {
                            type: 'component',
                            componentName: 'testComponent',
                            componentState: { label: 'A' }
                        },
                        {
                            type: 'column',
                            content: [
                                {
                                    type: 'component',
                                    componentName: 'testComponent',
                                    componentState: { label: 'B' }
                                },
                                {
                                    type: 'component',
                                    componentName: 'testComponent',
                                    componentState: { label: 'C'}
                                }
                            ]
                        }

                    ]
                }
            ]
        };
        var el = document.getElementById(containerName);
        var myLayout = new GoldenLayout(config, el);
        myLayout.registerComponent('testComponent', function (container, componentState) {
            container.getElement().html('<h2>' + componentState.label + '</h2>');
        });

        myLayout.init();
    }
}