#include <iostream>
#include <string>
#include <thread>
#include <vector>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <strings.h>
#include <algorithm>

using namespace std;

const int SERVERPORT = 27100;
const int clientSlots = 2560;
const int BUFFERSIZE = 2048;

struct clientStruct {
public:
    socklen_t clilen;
    sockaddr_in cli_addr;
    int clientSocket;
    clientStruct() {};
    clientStruct(int _newsockfd, sockaddr_in _cli_addr, socklen_t _clilen) {
        this->clilen = _clilen;
        this->cli_addr = _cli_addr;
        this->clientSocket = _newsockfd;
    };
};

void error(std::string str) {
    cout << str << endl;
    exit(60);
}

void ClientThread(clientStruct* client) {

    cout << "THREAD " << this_thread::get_id() << " CALLED my client is: " << client->clientSocket << endl;

    char clientBuffer[BUFFERSIZE];
    int n = 0;
    string clientTmp = "";

    //Read packet from client
    bzero(clientBuffer, BUFFERSIZE);
    if (n = read(client->clientSocket, clientBuffer, BUFFERSIZE) < 1) {
        cout << "RESET CAUGHT!";
        close(client->clientSocket);
        delete client;
        return;
    }

    //Convert to string
    std::string userInput = clientBuffer;
    cout << "Got: " << userInput << endl;

    //Decode packet from client


    //Logic

    //Send packet to client
    string data = "PING BOI";

    bzero(clientBuffer, BUFFERSIZE);
    if (send(client->clientSocket, data.c_str(), data.size(), 0) < 1) {
        cout << "RESET CAUGHT!";
        close(client->clientSocket);
        delete client;
        return;
    }

    //Disconnect Client
    cout << "Closing Thread " << this_thread::get_id() << endl;
    close(client->clientSocket);

    //Cleanup
    delete client;
}


//Inits Server And Handles Initaliation Connection Of Client
void server() {

    int sockfd, newsockfd, portno;
    socklen_t clilen;

    struct sockaddr_in serv_addr, cli_addr;
    int n;

    sockfd = socket(AF_INET, SOCK_STREAM, 0);
    if (sockfd < 0)
        error("ERROR opening socket");

    bzero((char*)&serv_addr, sizeof(serv_addr));

    serv_addr.sin_family = AF_INET;

    serv_addr.sin_addr.s_addr = INADDR_ANY;

    serv_addr.sin_port = htons(SERVERPORT);
    if (bind(sockfd, (struct sockaddr*)&serv_addr, sizeof(serv_addr)) < 0) {
        //KILL EVERYTHING SOMETHING IS WRONG
        cout << "Something is wrong as the port is used attmepting to kill other processes..." << endl;
        system("killall -9 reiCore");
        error("ERROR Could not bind to port");
    }
    else {
        cout << "Port: " << SERVERPORT << " Bound" << endl;
    }

    listen(sockfd, clientSlots);

    clilen = sizeof(cli_addr);
    
    //Accept Client Connection
    while (true)
    {
        cout << "Waiting For A New Client" << endl;

        newsockfd = accept(sockfd, (struct sockaddr*)&cli_addr, &clilen);

        cout << "Connection Recieved!" << endl;

        if (newsockfd < 0) {
            error("ERROR Cannot Accept Connection, Socket Error " + newsockfd);
        }
        else {
            new thread(ClientThread, new clientStruct(newsockfd, cli_addr, clilen));
        }
    }

}

int main() {

	server();

	return 0;
}