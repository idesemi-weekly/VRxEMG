"""Code modified from the example program to show how to read a multi-channel time series from LSL at https://github.com/OpenBCI/OpenBCI_GUI/blob/master/Networking-Test-Kit/LSL/lslStreamTest.py."""

from pylsl import StreamInlet, resolve_stream
from pynput.keyboard import Controller
import time

def main():
    # resolve an EMG stream on the lab network and notify the user
    print("Looking for an EMG stream...")
    streams = resolve_stream('type', 'EMG')
    inlet = StreamInlet(streams[0])
    print("EMG stream found!")

    # initialize thresholds and variables for storing time 
    time_thres = 2000
    prev_time = 0
    pinky_thres = .98
    pointer_thres = .6
    keyboard = Controller()

    while True:

        sample, timestamp = inlet.pull_sample()  # get EMG data sample and its timestamp

        curr_time = int(round(time.time() * 1000))  # get current time in milliseconds
        
        if (sample[0] >= pinky_thres) & (curr_time - time_thres > prev_time):  # if an EMG peak from channel 1 is detected and enough time has gone by since the last one, press key
            prev_time = curr_time  # update time
            keyboard.press('o')
            print("Pinky flexed")

        elif (sample[1] >= pointer_thres) & (curr_time - time_thres > prev_time):  # if an EMG peak from channel 2 is detected from and enough time has gone by since the last one, press key
            prev_time = curr_time  # update time
            #keyboard.press('p')

        elif (sample[2] >= pointer_thres) & (curr_time - time_thres > prev_time):  # if an EMG peak from channel 3 is detected and enough time has gone by since the last one, press key
            prev_time = curr_time  # update time

        elif (sample[3] >= pinky_thres) & (curr_time - time_thres > prev_time):  # if an EMG peak from channel 4 is detected and enough time has gone by since the last one, press key
            prev_time = curr_time  # update time
            
if __name__ == '__main__':
    main()