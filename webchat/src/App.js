import React, { useMemo } from 'react';
import { createDirectLine} from 'botframework-webchat';
import { Components } from 'botframework-webchat-component';
import ChatHeader from './ChatHeader';
import Grid  from '@mui/material/Grid';

const App = (props) => {
    const directLine = useMemo(() =>
        createDirectLine({ token: props.token }), [props]);  

    return directLine !== undefined
        ? <>
            <Components.Composer directLine={directLine}>
                <Grid container>
                    <ChatHeader />
                </Grid>
                <Grid container
                    style={{ position: 'absolute', bottom: '0', height:'90vh' }}>
                    <Components.BasicWebChat userID={props.userId} />
                </Grid>
            </Components.Composer>
        </>
        : <></>
}

export default App;
