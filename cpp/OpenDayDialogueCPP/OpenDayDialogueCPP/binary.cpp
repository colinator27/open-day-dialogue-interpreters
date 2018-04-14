#include "OpenDayDialogue.h"

#include <string>
#include <map>
#include <list>

/// INTERNAL AREA ///

static const ODDuint32 version = 4;
static bool initialized = false;
static bool isLittleEndian = true;

static ODDint16 readShort(ODDchar* data, ODDuint32& pos)
{
    ODDint16 n = 0;
    std::memcpy(&n, &(data[pos]), 2);
    if (!isLittleEndian)
    {
        char* p = (char*)&n;
        ODDint16 n2 = n;
        char* p2 = (char*)&n2;
        p[0] = p2[1];
        p[1] = p2[0];
    }
    pos += 2;
    return n;
}

static ODDuint16 readUnsignedShort(ODDchar* data, ODDuint32& pos)
{
    ODDuint16 n = 0;
    std::memcpy(&n, &(data[pos]), 2);
    if (!isLittleEndian)
    {
        char* p = (char*)&n;
        ODDuint16 n2 = n;
        char* p2 = (char*)&n2;
        p[0] = p2[1];
        p[1] = p2[0];
    }
    pos += 2;
    return n;
}

static ODDint32 readInteger(ODDchar* data, ODDuint32& pos)
{
    ODDint32 n = 0;
    std::memcpy(&n, &(data[pos]), 4);
    if (!isLittleEndian)
    {
        char* p = (char*)&n;
        ODDint32 n2 = n;
        char* p2 = (char*)&n2;
        p[0] = p2[3];
        p[1] = p2[2];
        p[2] = p2[1];
        p[3] = p2[0];
    }
    pos += 4;
    return n;
}

static ODDuint32 readUnsignedInteger(ODDchar* data, ODDuint32& pos)
{
    ODDuint32 n = 0;
    std::memcpy(&n, &(data[pos]), 4);
    if (!isLittleEndian)
    {
        char* p = (char*)&n;
        ODDuint32 n2 = n;
        char* p2 = (char*)&n2;
        p[0] = p2[3];
        p[1] = p2[2];
        p[2] = p2[1];
        p[3] = p2[0];
    }
    pos += 4;
    return n;
}

static ODDdouble readDouble(ODDchar* data, ODDuint32& pos)
{
    ODDdouble n = 0;
    std::memcpy(&n, &(data[pos]), 8);
    if (!isLittleEndian)
    {
        char* p = (char*)&n;
        ODDdouble n2 = n;
        char* p2 = (char*)&n2;
        p[0] = p2[7];
        p[1] = p2[6];
        p[2] = p2[5];
        p[3] = p2[4];
        p[4] = p2[3];
        p[5] = p2[2];
        p[6] = p2[1];
        p[7] = p2[0];
    }
    pos += 8;
    return n;
}

static std::string readZeroTermString(ODDchar* data, ODDuint32 dataLength, ODDuint32& pos)
{
    ODDuint32 tempPos = pos;
    while (tempPos < dataLength && data[tempPos] != '\0')
        tempPos++;
    ODDuint32 length = tempPos - pos;
    std::string ret(data + pos, length);
    pos += length;
    return ret;
}

static std::string readString(ODDchar* data, ODDint32 stringLength, ODDuint32& pos)
{
    std::string ret(data + pos, stringLength);
    pos += stringLength;
    return ret;
}

class ODD_Binary
{
public:
    ODD_Binary(ODDchar* data, ODDuint32 length)
    {
        loadState = ODD_STATE_INVALID_BINARY;
        ODDuint32 pos = 0;

        std::string header = readString(data, 4, pos);
        if (header != "OPDA")
        {
            loadState = ODD_STATE_INVALID_BINARY_HEADER;
            return;
        }
        if (readUnsignedInteger(data, pos) != version)
        {
            loadState = ODD_STATE_INVALID_BINARY_VERSION;
            return;
        }

        loadState = ODD_STATE_SUCCESS;
    }

    ~ODD_Binary()
    {
    }

    ODD_STATE loadState;
    std::map<ODDuint32, std::string> stringTable;
    std::map<ODDuint32, ODD_Value> valueTable;
    std::map<std::string, std::string> definitions;
    std::map<ODDuint32, ODD_Command> commandTable;
    std::map<std::string, ODDuint32> scenes;
    std::map<ODDuint32, ODDint32> labels;
    std::list<ODD_Instruction> instructions;
};

/// EXTERNAL AREA ///

ODD_API ODD_STATE ODD_API_SPEC ODD_Init()
{
    // Check endianess of this platform
    int i = 1;
    char* p = (char*)&i;
    if (p[0] == 1)
        isLittleEndian = true;
    else
        isLittleEndian = false;

    initialized = true;
    return ODD_STATE_SUCCESS;
}

ODD_API ODD_STATE ODD_API_SPEC ODD_Close()
{
    initialized = false;
    return ODD_STATE_SUCCESS;
}

ODD_API ODD_STATE ODD_API_SPEC ODD_Create_Binary(ODD_Binary** binary, ODDchar* buffer, ODDuint32 length)
{
    if (binary == NULL || buffer == NULL)
        return ODD_STATE_INVALID_PARAMETERS;

    *binary = new ODD_Binary(buffer, length);

    if ((*binary)->loadState != ODD_STATE_SUCCESS)
    {
        ODD_STATE state = (*binary)->loadState;
        delete *binary;
        *binary = NULL;
        return state;
    }

    return ODD_STATE_SUCCESS;
}

ODD_API ODD_STATE ODD_API_SPEC ODD_Free_Binary(ODD_Binary** binary)
{
    if (binary == NULL || (*binary) == NULL)
        return ODD_STATE_INVALID_PARAMETERS;

    delete *binary;
    return ODD_STATE_SUCCESS;
}