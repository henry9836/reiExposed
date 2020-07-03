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
const string SEPERATOR = "--";

enum PACKET {
    ACK,
    PACKAGE_SEND,
    PACKAGE_RECIEVE,
};

vector<string> extractData(string input) {
    vector<string> data;

    for (size_t i = 0; i < input.length(); i++)
    {
        //Push back 
        size_t cutPoint = input.find_first_of(SEPERATOR);
        data.push_back(input.substr(0, cutPoint));
        input = input.substr(cutPoint + SEPERATOR.length());
        cout << "CUT INPUT TO: " << input << endl;
    }

    cout << "FULL DATA" << endl;

    for (size_t i = 0; i < data.size(); i++)
    {
        cout << data[i] << endl;
    }


    return data;
}

/// <PACKET FORMATTING>
/// ACK
/// TYPE--
/// PACKAGE
/// TYPE--STEAMID--MSG--CURR--ITM1-ITM2-ITM3
/// </PACKET FORMATTING>

struct packetStruct {
public:
    PACKET type;
    int item1 = 0;
    int item2 = 0;
    int item3 = 0;
    int curr = 10;
    string msg = "Good luck!";

    packetStruct();

    packetStruct(string input) {
        //Decode
        vector<string> data = extractData(input);

        //Determine PACKET TYPE
        type = (PACKET)stoi(data[0]);

        switch (type)
        {
            case PACKAGE_SEND: {
                msg = data[2];
                curr = stoi(data[3]);
                item1 = stoi(data[4]);
                item2 = stoi(data[5]);
                item3 = stoi(data[6]);
                break;
            }
            case PACKAGE_RECIEVE:
            case ACK: {
                break;
            }
            default: {
                cout << "[WARN] Unknown Packet Type {" << type << "}" << endl;
                break;
            }
        }
    };

};

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
    packetStruct* packet;
    std::string data = "0--error";

    //Read packet from client
    bzero(clientBuffer, BUFFERSIZE);
    if (n = read(client->clientSocket, clientBuffer, BUFFERSIZE) < 1) {
        cout << "RESET CAUGHT!";
        close(client->clientSocket);
        delete client;
        return;
    }

    //Convert to string
    data = clientBuffer;
    cout << "Recieved: " << data << endl;

    //Decode packet from client
    packet = new packetStruct(data);

    //reset data
    data = "0--error";

    //Logic
    switch (packet->type)
    {
    case ACK: {
        data = to_string(ACK) + SEPERATOR + "pong";
        cout << data;
        break;
    }
    //Store
    case PACKAGE_SEND: 
    {
        break;
    }
    //Send
    case PACKAGE_RECIEVE:
    {
        break;
    }
    default: {
        cout << "[WARN] No PACKET TYPE Logic exists for {" << packet->type << "}" << endl;
        break;
    }
    }

    //Send packet to client

    bzero(clientBuffer, BUFFERSIZE);
    if (send(client->clientSocket, data.c_str(), data.size(), 0) < 1) {
        cout << "RESET CAUGHT!";
        close(client->clientSocket);
        delete client;
        delete packet;
        return;
    }

    //Disconnect Client
    cout << "Closing Thread " << this_thread::get_id() << endl;
    close(client->clientSocket);

    //Cleanup
    delete client;
    delete packet;
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