// eslint-disable-next-line @typescript-eslint/no-namespace
var HGR_DOM;
(function (HGR_DOM) {
    class ForgeAppDOM {
        static Ini() {
            ForgeAppDOM.SubscribeElement();
            console.log("ForgeAppDOM.INI");
        }
        static SubscribeElement() {
            const root = document.getElementById("myDiv");
            //SCROLL DEBUGING
            //root.addEventListener("scroll", function (e) {
            //    ScrollingRountines.ChangeElmentContent("logo", root.scrollTop.toString())
            //});
            //TOGGLE CLASSES ON MODULE APEARENCE
            {
                const parents = [...root.getElementsByClassName("project-cover")].map(x => x);
                for (let p = 0; p < parents.length; p++) {
                    const parent = parents[p];
                    const header = parent.getElementsByClassName("projectHeader")[0];
                    ScrollingRountines.ChangeChildrenAnimationByScrolling(root, parent, header, "hideContentHeader", "showContentHeader");
                    header.style.fontSize = "110px";
                    const projectsInfo = parent.getElementsByClassName("projectInfo")[0];
                    ScrollingRountines.ChangeChildrenAnimationByScrolling(root, parent, projectsInfo, "hideContentInfo", "showContentInfo");
                }
            }
            //CHANGE THE HEADER AND THE PANE BACKGROUND ON FREESPACE MOUSEOVER
            {
                const modules = root.getElementsByClassName("horizontalModule");
                //console.log(modules);
                for (let m = 0; m < modules.length; m++) {
                    const module = modules[m];
                    const rail = module.parentElement;
                    const header = rail.getElementsByClassName("projectHeader")[0];
                    const info = rail.getElementsByClassName("projectInfo")[0];
                    const pane = module.getElementsByClassName("paneInteraction")[0];
                    const freeSpace = module.getElementsByClassName("freeZone")[0];
                    if (freeSpace) {
                        freeSpace.addEventListener("mouseover", function (e) {
                            //console.log("mouseover a freezone");
                            header.style.fontSize = "50px";
                            pane.style.opacity = "0.2";
                        });
                        freeSpace.addEventListener("mouseout", function (e) {
                            //console.log("mouseout a freezone");
                            header.style.fontSize = "90px";
                            pane.style.opacity = "1.0";
                        });
                    }
                }
            }
            //IMAGE RAIL
            {
                const horizotalRails = root.getElementsByClassName("horizontalRail");
                //console.log("horizontal Rails : " + horizotalRails.length);
                for (let r = 0; r < horizotalRails.length; r++) {
                    const rail = horizotalRails[r];
                    const modules = rail.getElementsByClassName("horizontalModule");
                    //console.log("   modules : " + modules.length, modules);
                    for (let m = 0; m < modules.length; m++) {
                        const indexIni = m - 1 >= 0 ? m - 1 : modules.length - 1;
                        const previousModule = modules[indexIni];
                        const currentModule = modules[m];
                        const indexEnd = m + 1 <= modules.length - 1 ? m + 1 : 0;
                        const nextModule = modules[indexEnd];
                        const rollBack = currentModule.getElementsByClassName("rollBack")[0];
                        const rollFront = currentModule.getElementsByClassName("rollFront")[0];
                        if (rollBack && previousModule) {
                            rollBack.addEventListener("click", function (e) {
                                console.log("onclick back", previousModule);
                                previousModule.scrollIntoView({ block: "start", behavior: "smooth" });
                                ScrollingRountines.ScrollToElement(rail, previousModule);
                            });
                        }
                        if (rollFront && nextModule) {
                            rollFront.addEventListener("click", function (e) {
                                console.log("onclick front", nextModule);
                                nextModule.scrollIntoView({ block: "start", behavior: "smooth" });
                                ScrollingRountines.ScrollToElement(rail, previousModule);
                            });
                        }
                    }
                }
            }
            const covers = [...root.getElementsByClassName("project-cover")].map(x => x);
            for (let c = 0; c < covers.length; c++) {
                const cover = covers[c];
                const info = cover.getElementsByClassName("projectInfo")[0];
                if (info === null)
                    continue;
                const modules = [...cover.getElementsByClassName("horizontalModule")].map(x => x);
                //console.log(modules);
                for (let i = 0; i < modules.length; i++) {
                    const module = modules[i];
                    //const imgPath = module.style.backgroundImage.replace('url(', '').replace(')', '').replace('"', '').replace('"', '');
                    //console.log(imgPath);
                    const observer = new IntersectionObserver(function (entries) {
                        const module = entries[0].target;
                        const backColor = module.getAttribute("backColor");
                        if (entries[0].isIntersecting === true && entries[0].intersectionRatio >= 0.5) {
                            const backs = document.getElementsByClassName("imageRibbon");
                            for (let i = 0; i < backs.length; i++) {
                                const background = backs[i];
                                if (backColor !== null && backColor.length > 0) {
                                    background.style.backgroundColor = "rgb(" + backColor + ")";
                                }
                            }
                            const paneInter = document.getElementsByClassName("paneInteraction");
                            for (let i = 0; i < paneInter.length; i++) {
                                const background = paneInter[i];
                                if (backColor !== null && backColor.length > 0) {
                                    background.style.backgroundImage = `linear-gradient(to bottom, rgba(${backColor},1), transparent, rgba(${backColor},1))`;
                                }
                            }
                            const commentNode = module.getElementsByClassName("moduleComments")[0];
                            if (commentNode !== null) {
                                if (module.classList.contains("firstModule")) {
                                    ScrollingRountines.ToggleClasses(info, "showContentInfo", "bounceContentInfo");
                                }
                                else if (info.classList.contains("bounceContentInfo")) {
                                    ScrollingRountines.ToggleClasses(info, "bounceContentInfo", "bounceContentInfo");
                                }
                                else {
                                    ScrollingRountines.ToggleClasses(info, "showContentInfo", "bounceContentInfo");
                                }
                                const content = commentNode.innerHTML;
                                info.innerHTML = content;
                                //console.log("BEFORE BOUNCE");
                            }
                        }
                        //pause and play videos
                        const videos = module.getElementsByTagName("video");
                        if (videos.length > 0) {
                            const vid = videos[0];
                            //const src = vid.getElementsByTagName("source")[0].src;
                            //console.log(src + " : " + entries[0].intersectionRatio.toString());
                            if (entries[0].intersectionRatio >= 0.5) {
                                vid.play();
                                //console.log("play video : " + src);
                            }
                            else {
                                vid.pause();
                                const root = document.getElementById("myDiv");
                                //console.log("pause video : " + src);
                            }
                        }
                    }, { threshold: [0, 0.25, 0.5, 0.75, 1] });
                    observer.observe(module);
                }
                //let videos = document.querySelectorAll('video');
                //let isPaused = false; /* Flag for auto-paused video */
                //let observer = new IntersectionObserver((entries, ooo) => {
                //    entries.forEach(entry => {
                //        let video = entry.target as HTMLElement;
                //        if (entry.intersectionRatio != 1 && !video.paused) {
                //            video.pause(); isPaused = true;
                //        }
                //        else if (isPaused) { video.play(); isPaused = false }
                //    });
                //}, { threshold: 1 });
                //observer.observe(videos);
            }
        }
        static SubscriveVideoHover() {
            const videos = document.getElementsByClassName("showControlsOnHover");
            for (var i = 0; i < videos.length; i++) {
                const video = videos[i];
                video.removeAttribute("controls");
                video.addEventListener("mouseenter", function (e) {
                    video.setAttribute("controls", "controls");
                });
                video.addEventListener("mouseleave", function (e) {
                    video.removeAttribute("controls");
                });
            }
        }
        static ReloadVideo(videoElId) {
            try {
                const el = document.getElementById(videoElId);
                if (el !== null) {
                    el.load();
                }
            }
            catch (e) {
            }
        }
    }
    class Util_DOM_Debug {
        static PrintTreeInfo(element) {
            let el = element;
            let elements = [];
            while (el != null) {
                elements.push(el);
                if (el.parentElement !== undefined) {
                    el = el.parentElement;
                }
                else {
                    break;
                }
            }
            let count = 0;
            for (var i = elements.length - 1; i >= 0; i--) {
                //console.log(count, elements[i], elements[i].offsetTop, elements[i].clientHeight);
                count++;
            }
        }
    }
    class ScrollingRountines {
        static ChangeElmentContent(id, content) {
            const el = document.getElementById(id);
            if (el) {
                el.innerHTML = content;
            }
        }
        static ContentVisibilityOnScroll(el) {
            const position = el.getBoundingClientRect();
            const top = position.top;
        }
        /**
         * Subscribe children nodes that satisfy the filterClass inside a html parent that its scrollabe
        toggleing classes
        */
        static ChangeChildrenAnimationByScrolling(root, parent, child, initialClass, finalClass) {
            const rootMid = (root.clientHeight / 2) + root.offsetTop;
            const parentOffset = ScrollingRountines.GetOffsetTopFromParent(root, parent);
            const parentMiddle = parentOffset + (parent.clientHeight / 2);
            const boundary = parentMiddle - rootMid - 5; /*5 px for pushing a little before get near the boundary*/
            root.addEventListener("scroll", function (e) {
                ScrollingRountines.ToggleClassessByScrolling(child, initialClass, finalClass, root, boundary);
            });
        }
        static ToggleClassessByScrolling(el, originalClass, toggleClass, parent, offsetHeight) {
            if (el.classList.contains(originalClass) || el.classList.contains(toggleClass)) {
                if (parent.scrollTop > offsetHeight) {
                    el.classList.remove(originalClass);
                    el.classList.add(toggleClass);
                }
                else {
                    el.classList.remove(toggleClass);
                    el.classList.add(originalClass);
                }
            }
        }
        static ToggleClasses(el, originalClass, toggleClass) {
            if (el.classList.contains(originalClass)) {
                el.classList.remove(originalClass);
                void el.offsetWidth;
                el.classList.add(toggleClass);
            }
            else if (el.classList.contains(toggleClass)) {
                el.classList.remove(toggleClass);
                void el.offsetWidth;
                el.classList.add(originalClass);
            }
        }
        static GetOffsetTopFromParent(parent, child) {
            let height = 0.00;
            let c = child;
            while (true) {
                if (c.isSameNode(parent)) {
                    break;
                }
                else {
                    height += c.offsetTop;
                    //console.log(c.offsetParent);
                    if (c.offsetParent) {
                        c = c.offsetParent;
                    }
                    else {
                        break;
                    }
                }
            }
            return height;
        }
        static ScrollToElement(parent, el) {
            parent.click = () => {
                console.log("ScrollToElement ...");
                el.scrollIntoView();
            };
        }
    }
    //Is a must, classes are needed in the windows object for consuming
    function Load() {
        window["ForgeAppDOM"] = ForgeAppDOM;
        window["Util_DOM_Debug"] = Util_DOM_Debug;
        window["ScrollingRountines"] = ScrollingRountines;
    }
    HGR_DOM.Load = Load;
})(HGR_DOM || (HGR_DOM = {}));
console.log("enter file.js");
HGR_DOM.Load();
//# sourceMappingURL=file.js.map