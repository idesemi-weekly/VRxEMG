import asyncio
import socket

async def udp_client():
    server_address = ('localhost', 12345)
    message = '{"type":"emg", "data":[0.99,0.99,0.023273082,0.04411545,0.8]}'
    await asyncio.sleep(0.1)

    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as sock:
        try:
            print(f"Sent {message} to {server_address}")
            sent = sock.sendto(message.encode(), server_address)

        except Exception as e:
            print(f"Error : {e}")

if __name__ == "__main__":
    asyncio.run(udp_client())