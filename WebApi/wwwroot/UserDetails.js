function revealUpdating() {
    const updating = document.getElementById("updating");
    updating.style.display = "flex";
}
const user = JSON.parse(sessionStorage.getItem("user"));
console.log(user)
const theName = document.querySelector("#theName")
theName.textContent = user.firstName;

const userEmail = document.querySelector("#newUserName");
const pass = document.querySelector("#newPassword");
const firstName = document.querySelector("#firstName");
const lastName = document.querySelector("#lastName");
const message = document.querySelector("#er2");

userEmail.value = user.email;

pass.value = user.password;
firstName.value = user.firstName;
lastName.value = user.lastName;

async function updateUser() {
   

    if (!userEmail.value || !pass.value || !lastName.value || !firstName.value) {
        message.textContent = "אנא מלא את כל השדות";
        return;
    }
    console.log(user)
    const newUserDetailes = {
        UserID: user.userID,
        Email: userEmail.value,
        Password: pass.value,
        FirstName: firstName.value,
        LastName: lastName.value,
    }
    try {
        const requestPass = "/api/users/"+ newUserDetailes.UserID.toString();
        const responseOurPost = await fetch(requestPass, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(newUserDetailes)
        });
       
        if (!responseOurPost.ok) {
            if (responseOurPost.text = "weak password")
                message.textContent = "הסיסמה חלשה מידי!";
            else 
                message.textContent = "אירעה שגיאה במהלך העידכון";
            throw "Error Updating"
        }
        message.textContent = "העדכון בוצע בהצלחה";
    }
    catch (e) {
        console.log(e.message)
    }
}

async function checkPass() {


    const pass = document.querySelector("#newPassword").value;
    if (pass == "") return;
    const progress = document.querySelector("#progress");
    const response = await fetch("/api/users/CheckPassword", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(pass)
    });
    const progVal = await response.json();
    progress.value = Number.parseInt(progVal) + 1;
    progress.style.display = "flex";

}
