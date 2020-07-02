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

int SERVERPORT = 27100;

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

    listen(sockfd, 32);

    clilen = sizeof(cli_addr);

    while (true)
    {

        cout << "Waiting For Connection..." << endl;

        newsockfd = accept(sockfd, (struct sockaddr*)&cli_addr, &clilen);

        cout << "Connection Found!" << endl;

        if (newsockfd < 0) {
            error("ERROR Cannot Accept Connection");
        }
        else {
            //new thread(ClientThread, new clientStruct(newsockfd, cli_addr, clilen));
            cout << "Connection Closed Cause Debgging" << endl;
        }
    }

}

int main() {

	server();

	return 0;
}