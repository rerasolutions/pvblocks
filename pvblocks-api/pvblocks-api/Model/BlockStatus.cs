using System.Collections.Generic;

namespace pvblocks_api.Model
{
    public class BlockStatus
    {
        public BlockStatus(int blockType, byte token, byte mode,  List<byte> statusBytes)
        {
            BlockType = blockType;
            Token = token;
            Mode = mode;
            StatusBytes = statusBytes;
        }

        public byte Token { get; set; }
        public byte Mode { get; set; }
        public int BlockType { get; set; }
        public List<byte> StatusBytes { get; set; }
    }
}
