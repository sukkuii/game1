n = int(input())
maxi = 0
for i in range(n):
    a = int(input())
    if a % 4 == 0 and a > maxi:
        maxi = a
print(maxi)