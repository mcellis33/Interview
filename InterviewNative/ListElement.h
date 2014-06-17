#include <stdlib.h>

template <typename T>
class ListElement
{
public:
    // compiler-generated copy constructor and assignment operator are OK,
    // because member-wise copy/assignment works
    ListElement(const T& value);
    T value();
    void setNext(ListElement<T>* next);

    ListElement<T>* getNext();

private:
    T _value;
    ListElement<T>* _next;
};


// IMPLEMENTATION

template <typename T>
ListElement<T>::ListElement(const T& value)
    : _value(value), _next(NULL)
{
}

template <typename T>
T ListElement<T>::value()
{
    return _value;
}

template <typename T>
void ListElement<T>::setNext(ListElement<T>* next)
{
    _next = next;
}

template <typename T>
ListElement<T>* ListElement<T>::getNext()
{
    return _next;
}