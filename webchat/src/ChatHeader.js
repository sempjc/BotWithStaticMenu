import React, { useCallback, useState } from 'react';
import { hooks } from 'botframework-webchat';
import {
    AppBar,
    Toolbar,
    IconButton,
    Menu,
    MenuItem
} from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';

const ChatHeader = () => {
    const [anchorEl, setAnchorEl] = useState(null);
    const handleClick = (event) => setAnchorEl(event.currentTarget);
    const handleClose = () => setAnchorEl(null);

    const sendMessage = hooks.useSendPostBack();
    const handleMenuItem = useCallback(option => {
        handleClose();
        return sendMessage(option);
    }, [sendMessage]);

    return <>
        <AppBar position='fixed'>
            <Toolbar>
                <IconButton
                    onClick={handleClick}
                    color='inherit'
                    edge='start'>
                    <MenuIcon/>
                </IconButton>
                <Menu
                    id='simple-menu'
                    anchorEl={anchorEl}
                    keepMounted
                    open={Boolean(anchorEl)}
                    onClose={handleClose} >
                    <MenuItem
                        onClick={() => handleMenuItem('goto:menu')}>
                        Menu
                    </MenuItem>
                    <MenuItem
                        onClick={() => handleMenuItem('goto:userprofiledialog')}>
                        UserProfile
                    </MenuItem>
                    <MenuItem
                        onClick={() => handleMenuItem('goto:weatherdialog')}>
                        Weather
                    </MenuItem>
                    <MenuItem
                        onClick={() => handleMenuItem('goto:cancelconversation')}>
                        Cancel Conversation
                    </MenuItem>
                </Menu>
            </Toolbar>
        </AppBar>
    </>
}

export default ChatHeader;
