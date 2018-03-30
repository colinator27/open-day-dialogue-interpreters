#ifndef OPEN_DAY_DIALOGUE
#define OPEN_DAY_DIALOGUE

/// BEGIN PREPROCESSOR DEFINITIONS ///

#ifndef ODD_API
# if defined(ODD_STATIC)
#  define ODD_API
# elif defined(_WIN32)
#  if defined(ODD_BUILD)
#   define ODD_API __declspec(dllexport)
#  else
#   define ODD_API __declspec(dllimport)
#  endif
# else
#  define ODD_API extern
# endif
#endif

#if defined(_WIN32)
# define ODD_API_SPEC __cdecl
#else
# define ODD_API_SPEC
#endif

/// BEGIN TYPE DEFINITIONS ///

/* 1 byte, character */
typedef char ODDchar;

/* signed 32-bit integer */
typedef int ODDint32;

/* unsigned 32-bit integer */
typedef unsigned int ODDuint32;

/* 64-bit IEEE754 floating point number */
typedef double ODDdouble;

/// BEGIN CLASS DEFINITIONS ///

typedef
    struct ODD_Binary
        ODD_Binary;

typedef
    struct ODD_Value
        ODD_Value;

typedef
    struct ODD_Command
        ODD_Command;

typedef
    struct ODD_Instruction
        ODD_Instruction;

/// BEGIN ENUMERATOR DEFINITIONS ///

typedef unsigned short ODD_STATE;
#define ODD_STATE_SUCCESS 0x0000
#define ODD_STATE_FAILURE_UNKNOWN 0x0001
#define ODD_STATE_INVALID_PARAMETERS 0x0002
#define ODD_STATE_INVALID_BINARY 0x0003

/// BEGIN FUNCTION DEFINITIONS ///

#ifdef __cplusplus
extern "C" {
#endif

ODD_API ODD_STATE ODD_API_SPEC ODD_Init();
ODD_API ODD_STATE ODD_API_SPEC ODD_Close();
ODD_API ODD_STATE ODD_API_SPEC ODD_Create_Binary(ODD_Binary** binary, ODDchar* buffer, ODDuint32 length);
ODD_API ODD_STATE ODD_API_SPEC ODD_Free_Binary(ODD_Binary** binary);

#ifdef __cplusplus
} /* extern "C" */
#endif

#endif /* OPEN_DAY_DIALOGUE */