const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

connection.on("ReceiveMessage", (user, message) => {
    const msg = document.createElement('div');
    msg.textContent = `${user}: ${message}`;
    document.getElementById('messages').appendChild(msg);
});

connection.start().catch(err => console.error(err.toString()));

document.getElementById('sendButton').addEventListener('click', () => {
    const user = document.getElementById('userInput').value;
    const message = document.getElementById('messageInput').value;
    connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
});
