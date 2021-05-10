Module output format, each frame contains 12 bytes (hexadecimal)

	Byte0: 0x5A frame header flag
	Byte1: 0x5A frame header flag
	Byte2: 0x01 The data type of this frame (refer to the meaning description)
		Bit7-Bit5 reserved
		Bit4 Q4 represents the quaternion output flag bit. 
			1: Indicates that there is quaternion output; 
			0: Indicates that there is no quaternion output.
		Bit3 YRP stands for Euler angle output flag. 
			1: indicates that there is Euler angle output; 
			0: indicates that there is no Euler angle output.
		Bit2 GYR represents the gyroscope output flag bit. 
			1: Indicates that there is gyro data output; 
			0: Indicates that there is no gyro data output.
		Bit1 MAG stands for the output flag of the magnetometer. 
			1: Indicates that there is magnetometer data output; 
			0: Indicates that there is no magnetometer data output.
		Bit0 ACC represents the acceleration output flag bit. 
			1: indicates that there is acceleration data output; 
			0: indicates that there is no acceleration data output.
	Byte3: 0x07 data volume
	Byte4: 0x00~0xFF The first high 8 bits of data
	Byte5: 0x00~0xFF The first low 8 bits of data
	Byte6: 0x00~0xFF The second high 8 bits of the data
	Byte7: 0x00~0xFF The second low 8 bits of the data
	Byte8: 0x00~0xFF The third high 8 bits of data
	Byte9: 0x00~0xFF The third low 8 bits after the data
	Byte10: 0x00~0xFF 8-bit data
		Bit7-Bit6 SYS represents the accuracy of the entire module: 
			0-3, the larger the value, the better, when a magnetic object approaches the module, the accuracy will decrease
		Bit5-Bit4 GYR indicates the accuracy of gyroscope calibration: 
			0-3, the larger the value, the better, and the module will be automatically calibrated when it is placed in a static state
		Bit3-Bit2 ACC indicates the accuracy of accelerometer calibration: 
			0-3, the larger the value, the better. The calibration method is to slowly hold the 6 sides of the module horizontally for a period of time
		Bit1-Bit0 MAG indicates the accuracy of magnetometer calibration: 
			0-3, the larger the value, the better, the calibration method is rotation around each axis or 8-character calibration
	Byte11: 0x00~0xFF checksum 
		Accumulative sum of the previous data, leaving only the lower 8 bits

Data calculation method
	
	Acceleration calculation method (byte2 & 0x01 = 0x01):
		ACC = (High 8 bits) << 8) | (Low 8 bits)		Unit m/s²
	Example: One frame of data
		< 5A-5A-01-07-03-27-FF-00-01-DD-F7-BA >
		Acc_X=(0x03<<8)|0x27=807 (m/s²)
		Acc_Y=(0xFF<<8)|0x00=-256(m/s²)
		Acc_Z=(0x01<<8)|0xDD=477 (m/s²)

	Magnetometer data calculation method (byte2 & 0x02 = 0x02):
		MAG = (High 8 bits) << 8) | (Low 8 bits)		Unit LSB (1μT = 16LSB)
	Example: One frame of data
		<5A-5A-02-07-FF-A0-FF-BD-FD-B5-F7-C1>
		Mag_X=(0xFF<<8)|0xA0=-96 (LSB)
		Mag_Y=(0xFF<<8)|0xBD=-67 (LSB)
		Mag_Z=(0xFD<<8)|0XB5=-587(LSB)

	Gyroscope data calculating method (byte2 & 0x04 = 0x04):
		GYRO = (High 8 bits) << 8) | (Low 8 bits)		Unit LSB (1Dps = 16LSB)
	Example: One frame of data
		<5A-5A-04-07-00-02-00-03-FF-FF-F7-B9>
		Gyr_X=(0x00<<8)|0x02=2 (LSB)
		Gyr_Y=(0x00<<8)|0x03=3 (LSB)
		Gyr_Z=(0xFF<<8)|0XFF=-1(LSB)

	Euler angle calculation method (byte2 & 0x08 = 0x08):
		EULER = (High 8 bits) << 8) | (Low 8 bits)		Unit LSB (1degree = 100LSB)
	Example: One frame of data
		<5A-5A-08-07-59-8D-17-25-03-8A-F7-69>
		Yaw=(0x59<<8)|0x8D=22925 (LSB)
		Roll=(0x17<<8)|0x25=5925 (LSB)
		Pitch=(0x03<<8)|0X8A=906 (LSB)

	Quaternion data calculation method (byte2&0x10=0x10):
		QUAT = (High 8 bits) << 8) | (Low 8 bits)		Unit LSB (1 = 10000LSB)
	Example: One frame of data
		<5A-5A-10-09-0C-77-12-B6-F9-2C-1F-34-F7-87>
		Q1=(0x0C<<8)|0x77=3191 (LSB)
		Q2=(0x12<<8)|0Xb6=4790 (LSB)
		Q3=(0xF9<<8)|0X2C=-1748(LSB)
		Q4=(0x1F<<8)|0X34=7988 (LSB)


Note: When multiple data are output, the output sequence of the data is ACC, MAG, GYR, YRP, Q4, SGAM (calibration data)
	For example: when Byte2=0x18, bit4 is 1 means there is quaternion output, and bit3 is 1 means there is Euler Angle output.
	Example: Analyze a frame of data below:
		<5A-5A-18-0F-4E-CF-FE-CE-FE-AF-07-5F-FF-31-01-58-26-54-F3-CD>
		
		Frame header: 5A5A
		Function byte: 18 means there are quaternion and Euler angle data output
		Data volume: 0F means a total of 15 data
		Data: 4ECF FECE FEAF 075F FF31 0158 2654 F3
		According to the order of data output, the first 6 bytes are YRP, the next 8 bytes are Q4, and the last word
		Section is the calibration status of the module

			Yaw=(0x4E<<8)|0xCF=20175 (LSB)
			Roll=(0xFE<<8)|0xCE=-306 (LSB)
			Pitch=(0xFE<<8)|0XAF=-337 (LSB)
		Euler angle data conversion unit 1 degree=100LSB

			Q1=(0x07<<8)|0x5F=1887 (LSB)
			Q2=(0xFF<<8)|0X31=-207 (LSB)
			Q3=(0x01<<8)|0X58=344 (LSB)
			Q4=(0x26<<8)|0X54=9812 (LSB)
		Quaternion data conversion 1=10000LSB (that is, the uploaded data is enlarged by 10000 times)

		SGAM=0xF3 means SYS calibration accuracy is 3, GYR calibration accuracy is 3, ACC calibration accuracy is 0, MAG calibration accuracy is 3


Command byte, sent by external controller to GY-955 module (hexadecimal)

	serial port output configuration register: 
		Byte0: 0XAA frame header flag
		Byte1: Command
			Bit7 (default 1) AUTO
				1: output according to the last output configuration after power-on; 
				0: no automatic output after power-on
			Bit6 (default 0) 100hz
				1: The continuous output frequency is about 100hz; 
				0 is not configured (when the baud rate is 115200)
			Bit5 (default 1) 50hz
				1: The continuous output frequency is about 50hz; 
				0 means no configuration (when the baud rate is 115200)
			Bit4 (default 1) Q4
				1: output quaternion data; 
				0 means not output quaternion data
			Bit3 (default 1) EULER
				1: Output Euler angle data; 
				0 means not output Euler angle data
			Bit2 (default 1) GYRO
				1: output gyroscope data; 
				0 means not output gyroscope data
			Bit1 (default 1) MAG
				1: Output magnetometer data; 
				0 means not output magnetometer data
			Bit0 (default 1) ACC
				1: Output accelerometer data; 
				0 means not output accelerometer data
		Byte2: Checksum (8bit)
	
		Example: Send command 
			<0xAA-0xA1-0x4B>
			Command = 0xA1, bit7(Auto=1), bit5(50hz output frequency=1), bit0(ACC=1)
			which means the accelerometer data is output continuously, and the accelerometer data will be output continuously after power on
	query output command:
		Byte0: 0xA5 frame header flag
		Byte1: Command
			0x15 : output accelerometer data (the data type returned by the module is 0x01)
			0x25 : output gyroscope data (the data type returned by the module is 0x04)
			0x35 : output magnetometer data (the module returns data type 0x02)
			0x45 : output Euler angle data (the module returns data type 0x08)
			0x55 : output quaternion (the data type returned by the module is 0x10)
		Byte2: Checksum (8bit)

		Note: The query command will not be saved after power-off. If you use query output, please pay attention to whether command=0x00 is configured before this.

	baud rate configuration:
	Byte0: 0xA5 frame header flag
		Byte1: Command
			0xAE : 9600 (default)
			0xAF : 115200
		Byte2: Checksum (8bit)



