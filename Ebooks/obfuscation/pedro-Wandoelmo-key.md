<br />

# SIMPLE STRING OBFUSCATION TECHNICS [2]<br />

- using one batch local variable to serve as our **master key** (varObj)<br />
`varObj=abcdefghijlmnopqrstuvxzkyW0123456789ABCDEFGHIJLMNOPQRSTUVXZKYW`<br />

- Then we just need to call the letter possition inside the **varObj** local variable to build our command<br />
**examples:** `%varObj:~2,1%` (letter c) **+** `%varObj:~11,1%` (letter m) **+** `%varObj:~3,1%` (letter d) **=** `cmd`<br />

---

**Letter/Number - Position inside varObj var**

a - 0,1<br />
b - 1,1<br />
c - 2,1<br />
d - 3,1<br />
e - 4,1<br />
f - 5,1<br />
g - 6,1<br />
h - 7,1<br />
i - 8,1<br />
j - 9,1<br />
l - 10,1<br />
m - 11,1<br />
n - 12,1<br />
o - 13,1<br />
p - 14,1<br />
q - 15,1<br />
r - 16,1<br />
s - 17,1<br />
t - 18,1<br />
u - 19,1<br />
v - 20,1<br />
x - 21,1<br />
z - 22,1<br />
k - 23,1<br />
y - 24,1<br />
w - 25,1<br />
<br />
0 - 26,1<br />
1 - 27,1<br />
2 - 28,1<br />
3 - 29,1<br />
4 - 30,1<br />
5 - 31,1<br />
6 - 32,1<br />
7 - 33,1<br />
8 - 34,1<br />
9 - 35,1<br />
<br />
A - 36,1<br />
B - 37,1<br />
C - 38,1<br />
D - 39,1<br />
E - 40,1<br />
F - 41,1<br />
G - 42,1<br />
H - 43,1<br />
I - 44,1<br />
J - 45,1<br />
L - 46,1<br />
M - 47,1<br />
N - 48,1<br />
O - 49,1<br />
P - 50,1<br />
Q - 51,1<br />
R - 52,1<br />
S - 53,1<br />
T - 54,1<br />
U - 55,1<br />
V - 56,1<br />
X - 57,1<br />
Z - 58,1<br />
K - 59,1<br />
Y - 60,1<br />
W - 61,1<br />
<br />

---

This document belongs to this article:<br />
https://github.com/r00t-3xp10it/hacking-material-books/blob/master/obfuscation/simple_obfuscation.md<br />
### Special thanks to: @Wandoelmo Silva

<br />

