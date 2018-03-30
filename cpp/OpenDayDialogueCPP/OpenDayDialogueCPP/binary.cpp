#include "OpenDayDialogue.h"

#include <string>
#include <map>
#include <list>

/// INTERNAL AREA ///

static int readInteger(char* data, int& pos)
{
    int n = (data[pos] | (data[pos + 1] << 8) | (data[pos + 2] << 16) | ((unsigned)data[pos + 3] << 24));
    pos += 4;
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
        isProperlyLoaded = false;
        ODDuint32 pos = 0;

        std::string header = readString(data, 4, pos);
        if (header != "OPDA")
            return;

        isProperlyLoaded = true;
    }

    ~ODD_Binary()
    {
    }

    bool isProperlyLoaded;
    std::map<ODDuint32, std::string> stringTable;
    std::map<ODDuint32, ODD_Value> valueTable;
    std::map<std::string, std::string> definitions;
    std::map<ODDuint32, ODD_Command> commandTable;
    std::map<std::string, ODDuint32> scenes;
    std::map<ODDuint32, ODDint32> labels;
    std::list<ODD_Instruction> instructions;
};

static bool initialized = false;

/// EXTERNAL AREA ///

ODD_API ODD_STATE ODD_API_SPEC ODD_Init()
{
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

    if ((*binary)->isProperlyLoaded == false)
    {
        delete *binary;
        *binary = NULL;
        return ODD_STATE_INVALID_BINARY;
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