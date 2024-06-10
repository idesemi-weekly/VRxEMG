import socket

def main():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    server_address = ('172.16.176.13', 5005)
    print('starting up on {} port {}'.format(*server_address))
    sock.bind(server_address)
    print(server_address)

    while True:
        print('\nwaiting to receive message')
        data = sock.recvfrom(4096)
        print(data)
            
if __name__ == '__main__':
    main()