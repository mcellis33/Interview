#include "LinkedList.h"
#include <iostream>
using namespace std;

int main ()
{
    LinkedList<int> l;

    cout << "l push all" << endl;
    for (int i = 0; i < 10; ++i)
        l.push_front(i);
    while (l.size() > 0)
        cout << l.pop_front() << endl;

    cout << "lCopy pop all" << endl;
    for (int i = 0; i < 10; ++i)
        l.push_front(i);

    LinkedList<int> lCopy(l);
    while (lCopy.size() > 0)
        cout << lCopy.pop_front() << endl;

    int dummy;
    cin >> dummy;
    return 0;
}