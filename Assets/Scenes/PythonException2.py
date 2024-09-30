user = [] 
while True:
    try:
        name = input("Enter your name: ")
        if len(name) >= 2:
            user.append(name.capitalize())
            break
        print("Name must have at least 2 characters.")
    except:
        print("Invalid name, Please try again.")

while True:
    try:
        age = int(input("Enter your age: "))
        if age >= 18:
            user.append(age)
            break
        print("You are too young.")
    except ValueError:
        print("Please enter a valid number.")

while True:
    try:
        username = input("Enter your username: ")
        
        if len(username) > 20:
            print("Username too long. Maximum length is 20 characters.")
        elif len(username) < 2:
            print("Username too short. Minimum length is 2 characters.")
        else:
            user.append(username)
            break
    except:
        print("Invalid username, Please try again.")


while True:
    password = input("Enter your password: ")
    if not any(char.isupper() for char in password):
        print("Password must contain at least 1 uppercase, 1 number, and 1 symbol.")
    elif not any(char.isdigit() for char in password):
        print("Password must contain at least 1 uppercase, 1 number, and 1 symbol.")
    elif not any(char in "!@#$%^&*()_+=-" for char in password):
        print("Password must contain at least 1 uppercase, 1 number, and 1 symbol.")
    else:
        user.append(password)
        break

for info in user:
    print([info]) 
