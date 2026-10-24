const revealSighning = () => {
    const divNew = document.querySelector("#newUser")
    divNew.style.display = "flex"
}       

async function login() {
    const userNameFromDoc = document.querySelector("#userName").value;
    const passwordFromDoc = document.querySelector("#password").value;
    if (!userNameFromDoc || !passwordFromDoc) {
        const er = document.querySelector("#er")
        er.textContent = "אנא מלא את כל השדות";
        return;
    }

    const userDetailes = {
        Email: userNameFromDoc,
        Password: passwordFromDoc,
    }
    const responseOurPost = await fetch("/api/users/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userDetailes)
    });
    if (!responseOurPost.ok) {
        const er = document.querySelector("#er")
        er.textContent = "אינך משתמש רשום במערכת, אנא הירשם";
        return
    }
    const user = await responseOurPost.json()
    console.log(user)
    sessionStorage.setItem("user", JSON.stringify(user))

    window.location.href = "UserDetailes.html"

}
async function signIn() {
    const userN = document.querySelector("#newUserName").value;
    const pass = document.querySelector("#newPassword").value;
    const firstName = document.querySelector("#firstName").value;
    const lastName = document.querySelector("#lastName").value;
    const er2 = document.querySelector("#er2")

    if (!userN || !pass || !lastName || !firstName) {
       er2.textContent = "אנא מלא את כל השדות";
       return;
    }

    const newUserDetailes = {
        Email: userN,
        Password: pass,
        FirstName: firstName,
        LastName: lastName,
    }
    const responseOurPost = await fetch("/api/users/signin", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(newUserDetailes)
    });
    if (!responseOurPost.ok) {
        er2.textContent = "אירעה שגיאה במהלך הרישום";
        return
    }
    er2.textContent = "הרישום בוצע בהצלחה, כעת תוכל להתחבר למערכת";
    userN.textContent = ""
    pass.textContent = ""
}