using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkDataTypes
{
    public class PacketBuffer : IDisposable
    {
        List<byte> _bufferList;
        byte[] _readBuffer;
        int _readPos;
        bool _buffUpdate = false;

        public PacketBuffer()
        {
            _bufferList = new List<byte>();
            _readPos = 0;
        }

        public PacketBuffer(byte[] initialBytes) : this()
        {
            WriteBytes(initialBytes);
        }

        public int GetReadPos()
        {
            return _readPos;
        }

        public byte[] ToArray()
        {
            return _bufferList.ToArray();
        }

        public int Count()
        {
            return _bufferList.Count;
        }

        public int Lenght()
        {
            return Count() - _readPos;
        }

        public void Clear()
        {
            _bufferList.Clear();
            _readPos = 0;
        }

        //WriteData
        public void WriteBytes(byte[] input)
        {
            _bufferList.AddRange(input);
            _buffUpdate = true;
        }

        public void WriteByte(byte input)
        {
            byte[] b = {input};
            WriteBytes(b);
        }
        
        public void WriteInt(int input)
        {
            byte[] bytes = BitConverter.GetBytes(input);

            if(BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            WriteBytes(bytes);
        }

        public void WriteFloat(float input)
        {
            WriteBytes(BitConverter.GetBytes(input));
        }

        public void WriteString(string input)
        {
            WriteInt(input.Length);
            WriteBytes(Encoding.ASCII.GetBytes(input));
        }

        //ReadData
        public int ReadInt(bool peek = true)
        {
            if(_bufferList.Count > _readPos)
            {
                if(_buffUpdate)
                {
                    _readBuffer = _bufferList.ToArray();
                    _buffUpdate = false;
                }

                byte[] value = _bufferList.GetRange(_readPos, 4).ToArray();

                if(peek & _bufferList.Count > _readPos)
                    _readPos += 4;

                if(BitConverter.IsLittleEndian)
                    Array.Reverse(value);
                
                return BitConverter.ToInt32(value, 0);
            }
            else
                throw new Exception("Nothing more to read");
        }

        public float ReadFloat(bool peek = true)
        {
            if(_bufferList.Count > _readPos)
            {
                if(_buffUpdate)
                {
                    _readBuffer = _bufferList.ToArray();
                    _buffUpdate = false;
                }
                float value = BitConverter.ToSingle(_readBuffer, _readPos);
                
                if(peek & _bufferList.Count > _readPos)
                    _readPos += 4;
                
                return value;
            }
            else
                throw new Exception("Nothing more to read");
        }

        public byte ReadByte(bool peek = true)
        {
            if(_bufferList.Count > _readPos)
            {
                if(_buffUpdate)
                {
                    _readBuffer = _bufferList.ToArray();
                    _buffUpdate = false;
                }
                byte value = _readBuffer[_readPos];
                
                if(peek & _bufferList.Count > _readPos)
                    _readPos += 1;
                
                return value;
            }
            else
                throw new Exception("Nothing more to read");
        }

        public byte[] ReadBytes(int count, bool peek = true)
        {
            if(_buffUpdate)
            {
                _readBuffer = _bufferList.ToArray();
                _buffUpdate = false;
            }
            byte[] value = _bufferList.GetRange(_readPos, count).ToArray();
            
            if(peek & _bufferList.Count > _readPos)
                _readPos += count;
            
            return value;
        }

        public string ReadString(bool peek = true)
        {
            int count = ReadInt();
            if(_buffUpdate)
            {
                _readBuffer = _bufferList.ToArray();
                _buffUpdate = false;
            }
            string value =  Encoding.ASCII.GetString(_readBuffer, _readPos, count);

            if(peek & _bufferList.Count > _readPos)
                _readPos += count;

            return value;
        }

        //IDisposable
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if(disposedValue == false)
            {
                if(disposing)
                    _bufferList.Clear();
                _readPos = 0;
            }
            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}